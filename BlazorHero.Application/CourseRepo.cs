using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using MIMS.Education.DTOs;
using MIMS.Education.Services;
using MIMS.Education.Entities;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using System.Collections.Generic;
using MIMS.Education.Common;
using MIMS.Education.Storage;

    public class CourseRepo : IRepository<CourseDTO>
    {
        DBContext Context { get; }
        IAdminService AdminService { get; }
        ILogRepo<CourseDTO> LogRepo { get; }

        #region Constructor

        public CourseRepo(DBContext context)
        {
            Context = context;

            InitializeAutoMapper();
        }

        public CourseRepo(DBContext context, IAdminService adminService, ILogRepo<CourseDTO> logRepo = null)
        {
            Context = context;
            AdminService = adminService;
            LogRepo = logRepo;

            InitializeAutoMapper();
        }
        #endregion


        #region AutoMapper
        MapperConfiguration dtoMap, entityMap, detailMap, certificateMap, quizMap;
        IMapper dtoMapper, entityMapper, detailMapper, certificateMapper, quizMapper;

        void InitializeAutoMapper()
        {
            entityMap = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CourseDTO, Course>();
                cfg.CreateMap<CertificateDTO, Certificate>();
            });
            entityMapper = entityMap.CreateMapper();

            dtoMap = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Course, CourseDTO>();
                cfg.CreateMap<Certificate, CertificateDTO>();
            });
            dtoMapper = dtoMap.CreateMapper();

            certificateMap = new MapperConfiguration(cfg => cfg.CreateMap<CertificateDTO, Certificate>());
            certificateMapper = certificateMap.CreateMapper();

            detailMap = new MapperConfiguration(cfg => { cfg.CreateMap<Course, CourseDetailDTO>(); });
            detailMapper = detailMap.CreateMapper();

            quizMap = new MapperConfiguration(cfg => { cfg.CreateMap<Course, QuizDTO>(); });
            quizMapper = quizMap.CreateMapper();

        }
        #endregion


        #region CRUD
        public IQueryable<CourseDTO> QueryAll()
        {
            return Context.Courses
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ProjectTo<CourseDTO>(dtoMap);
        }


        public List<Log> GetLogs(string key)
        {
            return LogRepo.GetAll(key).ToList();
        }


        public async Task<RepoResult> CreateAsync(CourseDTO dto)
        {
            var result = new RepoResult();
            var entity = new Course();

            //0. Validation
            if (dto == null)
            {
                result.Message = "Object can't be null";
                return result;
            }

            dto.CreatedAt = DateTime.Now;
            dto.UpdatedAt = DateTime.Now;
            dto.CreatedBy = AdminService?.Me?.Name ?? string.Empty;
            dto.UpdatedBy = AdminService?.Me?.Name ?? string.Empty;

            try
            {
                //1. Arrange
                entity = entityMapper.Map<Course>(dto);

                entity.Remarks = dto.CourseDetail.Remarks;
                entity.Content = dto.CourseDetail.Content;
                entity.Description = dto.CourseDetail.Description;

                entity.QuizIntro = dto.Quiz.QuizIntro;
                entity.QuizFooter = dto.Quiz.QuizFooter;
                entity.QuizContext = dto.Quiz.QuizContext;

                ////MEDU-180 Workaround solution for fixing wrongly insert new certificat error
                entity.Certificate = Context.Certificates.Find(dto.Certificate?.Id);

                //2. Act
                await Context.AddAsync(entity);
                result.Affected = await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }


            //3. Assert
            if (result.Affected > 0)
            {
                dto.Id = entity.Id;
                dto.CourseNo = entity.CourseNo;

                result.StatusCode = StatusCodes.Status201Created;
            }


            //4. Logging
            await LogRepo.SaveAsync(new Log()
            {
                PartitionKey = dto.Id.ToString(),
                Action = Actions.Create,
                Status = result.StatusCode,
                Affected = result.Affected,
                Remarks = result.Message,
                By = dto.UpdatedBy
            });

            dto.Action = null;

            return result;
        }


        public async Task<RepoResult> RemoveAsync(CourseDTO dto)
        {
            var result = new RepoResult();

            //0. Validate
            if (dto?.Id == null)
                return new RepoResult(StatusCodes.Status400BadRequest, 0, $"Item can't be null");


            //1. Arrange
            var entity = Context.Courses.Find(dto.Id);
            if (entity == null)
                return new RepoResult(StatusCodes.Status404NotFound, 0, $"Item not found!! (Id:{dto.Id})");

            //2. Act
            try
            {
                //Context.Remove(entity);
                entity.IsDeleted = true;
                entity.Status = CourseStatus.Deleted;
                entity.DeletedAt = DateTime.Now;

                result.Affected = await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }


            //3. Assert 
            if (result.Affected > 0)
                result.StatusCode = StatusCodes.Status204NoContent;


            //4. Logging
            await LogRepo.SaveAsync(new Log()
            {
                PartitionKey = dto.Id.ToString(),
                Action = Actions.Delete,
                Status = result.StatusCode,
                Affected = result.Affected,
                Remarks = result.Message,
                By = AdminService?.Me?.Name ?? string.Empty
            });


            return result;
        }


        public async Task<RepoResult> SaveAsync(CourseDTO dto)
        {
            var result = new RepoResult();

            //0. Validation
            if (dto == null)
            {
                result.Message = "Object can't be null";
                return result;
            }
            else if (dto.Id == Guid.Empty || dto.CreatedAt is null)
            {
                return await CreateAsync(dto);
            }


            //1.Arrange
            //var entity = await Context.Courses.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            var entity = await Context.Courses.FindAsync(dto.Id);
            if (entity is null)
                return new RepoResult(StatusCodes.Status404NotFound);


            //2. Act
            try
            {
                updateDTOtoEntity(dto, entity);

                if(AdminService is not null)
                {
                    dto.UpdatedBy = AdminService.Me.Name;
                    dto.UpdatedAt = DateTime.Now;
                }

                result.Affected = await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }

            //3. Assert 
            if (result.Affected > 0)
                result.StatusCode = StatusCodes.Status204NoContent;


            //4. Logging
            if(LogRepo is not null)
            {
                await LogRepo?.SaveAsync(new Log()
                {
                    PartitionKey = dto.Id.ToString(),
                    Action = dto.Action ?? Actions.Update,
                    Status = result.StatusCode,
                    Affected = result.Affected,
                    Remarks = result.Message,
                    By = AdminService?.Me?.Name ?? string.Empty
                });
            }

            dto.Action = null;

            return result;
        }



        private void updateDTOtoEntity(CourseDTO dto, Course entity)
        {
            entity.CountryCode = dto.CountryCode;
            entity.ModuleCode = dto.ModuleCode;
            entity.Sponsor = dto.Sponsor;

            entity.Profession = dto.Profession;
            entity.Sponsor = dto.Sponsor;
            entity.StartAt = dto.StartAt;
            entity.EndAt = dto.EndAt;
            entity.Length = dto.Length;
            entity.Specilties = dto.Specilties;

            entity.Thumbnail = dto.Thumbnail;
            entity.Title = dto.Title;
            entity.Introduction = dto.Introduction;
            entity.Description = dto.CourseDetail.Description;
            entity.Content = dto.CourseDetail.Content;
            entity.Remarks = dto.Remarks;
            entity.AttributionMode = dto.AttributionMode;

            entity.HavingCertificate = dto.HavingCertificate;
            entity.Certificate = Context.Certificates.Find(dto.Certificate?.Id);

            entity.HavingInterstitialAds = dto.CourseDetail.HavingInterstitialAds;
            entity.InterstitialAdsStartAt = dto.CourseDetail.InterstitialAdsStartAt;
            entity.InterstitialAdsEndAt = dto.CourseDetail.InterstitialAdsEndAt;
            entity.InterstitialAdsImgUrl = dto.CourseDetail.InterstitialAdsImgUrl;

            entity.IsLimitedSeats = dto.IsLimitedSeats;
            entity.SeatsQuota = dto.SeatsQuota;

            entity.HavingQuiz = dto.HavingQuiz;
            entity.MaxAttempts = dto.MaxAttempts;
            entity.PassingScore = dto.PassingScore;

            entity.ProgramCode = dto.ProgramCode;
            entity.ProgramProvider = dto.ProgramProvider;
            entity.CPDPoints = dto.CPDPoints;

            entity.QuizIntro = dto.Quiz.QuizIntro;
            entity.QuizFooter = dto.Quiz.QuizFooter;
            entity.QuizContext = dto.Quiz.QuizContext;

            entity.PublicUrl = dto.PublicUrl;
            entity.PrivateUrl = dto.PrivateUrl;

            entity.PublicEnrolled = dto.PublicEnrolled;
            entity.PrivateEnrolled = dto.PrivateEnrolled;
            entity.Status = dto.Status;

            entity.PublishedAt = dto.PublishedAt;
            entity.PausedAt = dto.PausedAt;
            entity.StoppedAt = dto.StoppedAt;
            entity.UpdatedAt = dto.UpdatedAt;
            entity.UpdatedBy = dto.UpdatedBy;
        }
        #endregion


        #region Indexing
        public Task<bool> IsExisted(CourseDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<CourseDTO> GetAsync(CourseDTO dto)
        {
            return await GetAsync(dto.Id.ToString());
        }

        public async Task<CourseDTO> GetAsync(string index)
        {
            //1. Arrange
            Course entity = null;

            //2. Act
            if (index.IsGuid())
            {
                var guid = Guid.Parse(index);
                entity = await Context.Courses
                    .Include(x => x.Certificate)
                    .Where(x => x.Id == guid)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            else if (index.IsInteger())
            {
                var no = int.Parse(index);
                entity = await Context.Courses
                    .Include(x => x.Certificate)
                    .Where(x => x.CourseNo == no || x.CourseHash == index)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            else if (index.IsHashCode())
            {
                entity = await Context.Courses
                    .Include(x => x.Certificate)
                    .Where(x => x.CourseHash == index)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }

            //3. Assert
            if (entity is null)
                return null;

            var dto = dtoMapper.Map<CourseDTO>(entity);
            dto.CourseDetail = detailMapper.Map<CourseDetailDTO>(entity);
            dto.Quiz = quizMapper.Map<QuizDTO>(entity);

            return dto;
        }
        #endregion
    }
