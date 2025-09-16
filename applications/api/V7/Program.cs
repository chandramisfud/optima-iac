using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Repositories.Contracts;
using Repositories.Contracts.Promo;
using Repositories.Repos;
using Repositories.Repos.Promo;
using V7.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

// AUTH Start
builder.Services.AddScoped<IAuthMenuRepository, AuthMenuRepository>();
builder.Services.AddScoped<IAuthUserRepository, AuthUserRepository>();
// Auth End

// TOOLS Start
builder.Services.AddScoped<IToolsFileRepository, ToolsFileRepository>();
builder.Services.AddScoped<IToolsEmailRepository, ToolsEmailRepository>();
builder.Services.AddScoped<IToolsBlitzRepository, ToolsBlitzRepository>();
builder.Services.AddScoped<IUploadRepo, UploadRepo>();
builder.Services.AddScoped<IXMLPaymentRepository, XMLPaymentRepo>();
builder.Services.AddScoped<ISchedulerRepo, SchedulerRepo>();

// Tools End

// DashBoard Start
builder.Services.AddScoped<IDashboardMainRepo, DashboardMainRepo>();
builder.Services.AddScoped<IDashboardSummaryRepo, DashboardSummaryRepo>();
builder.Services.AddScoped<IDashboardPromoCalendarRepo, DashboardPromoCalendarRepo>();
builder.Services.AddScoped<IDashboardApproverRepo, DashboardApproverRepo>();
builder.Services.AddScoped<IDashboardCreatorRepo, DashboardCreatorRepo>();

// DashBoard End

// DN Start
builder.Services.AddScoped<IDNCreationRepo, DNCreationRepo>();
builder.Services.AddScoped<IDNCreationHORepo, DNCreationHORepo>();
builder.Services.AddScoped<IDNSendtoHORepo, DNSendtoHORepo>();
builder.Services.AddScoped<IDNSuratJalanAndTandaTerimatoHORepo, DNSuratJalanAndTandaTerimatoHORepo>();
builder.Services.AddScoped<IDNReceivedandApprovedbyHORepo, DNReceivedandApprovedbyHORepo>();
builder.Services.AddScoped<IDNSendtoDanoneRepo, DNSendtoDanoneRepo>();
builder.Services.AddScoped<IDNSuratJalanAndTandaTerimatoDanoneRepo, DNSuratJalanAndTandaTerimatoDanoneRepo>();
builder.Services.AddScoped<IDNReceivedbyDanoneRepo, DNReceivedbyDanoneRepo>();
builder.Services.AddScoped<IDNValidationbyFinanceRepo, DNValidationbyFinanceRepo>();
builder.Services.AddScoped<IDNValidationbySalesRepo, DNValidationbySalesRepo>();
builder.Services.AddScoped<IDNInvoiceNotificationByDanoneRepo, DNInvoiceNotificationByDanoneRepo>();
builder.Services.AddScoped<IDNCreateInvoiceRepo, DNCreateInvoiceRepo>();
builder.Services.AddScoped<IDNConfirmDNPaidRepo, DNConfirmDNPaidRepo>();
builder.Services.AddScoped<IDNMultiPrintRepo, DNMultiPrintRepo>();
builder.Services.AddScoped<IDNUploadRepo, DNUploadRepo>();
builder.Services.AddScoped<IDNUploadAttachmentRepo, DNUploadAttachmentRepo>();
builder.Services.AddScoped<IDNListingPromoDistributorRepo, DNListingPromoDistributorRepo>();
builder.Services.AddScoped<IDNWorkflowRepo, DNWorkflowRepo>();
builder.Services.AddScoped<IDNReassignmentRepo, DNReassignmentRepo>();
builder.Services.AddScoped<IDNManualAssignmentRepo, DNManualAssignmentRepo>();
builder.Services.AddScoped<IDNListingOverBudgetRepo, DNListingOverBudgetRepo>();
builder.Services.AddScoped<IDNReassignmentbyFinanceRepo, DNReassignmentbyFinanceRepo>();
builder.Services.AddScoped<IDNSendBackRepo, DNSendBackRepo>();
builder.Services.AddScoped<IDNOverBudgetRepo, DNOverBudgetRepo>();
builder.Services.AddScoped<IDNVATExpiredChecklistRepo, DNVATExpiredChecklistRepo>();
builder.Services.AddScoped<IDNPromoDisplayRepo, DNPromoDisplayRepo>();
builder.Services.AddScoped<IDNMultiPrintPromoRepo, DNMultiPrintPromoRepo>();
builder.Services.AddScoped<IDNUploadFakturRepo, DNUploadFakturRepo>();
// DN End

// USER START
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserGroupMenuRepository, UserGroupMenuRepository>();
// USER END

// CONFIGURATION Start
builder.Services.AddScoped<IConfigRepository, ConfigRepository>();
builder.Services.AddScoped<ILatePromoCreationRepo, LatePromoCreationRepo>();
builder.Services.AddScoped<IPromoInitiatorReminderRepo, PromoInitiatorReminderRepo>();
builder.Services.AddScoped<IReminderRepo, ReminderRepo>();
builder.Services.AddScoped<IROIandCRRepo, ROIandCRRepo>();
builder.Services.AddScoped<IMajorChangesRepository, MajorChangesRepository>();
builder.Services.AddScoped<IPromoItemRepo, PromoItemRepo>();

// CONFIGURATION Emd

// USERACCESS Start
builder.Services.AddScoped<IUserLevelRepository, UserLevelRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAdminReportRepository, UserAdminReportRepository>();
// USERACCESS End

// MASTER Start
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IDistributorRepository, MasterDistributorRepository>();
builder.Services.AddScoped<IEntityRepository, MasterEntityRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICancelReasonRepository, CancelReasonRepository>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<IInvestmentTypeRepository, InvestmentTypeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IProfitCenterRepository, ProfitCenterRepository>();
builder.Services.AddScoped<ISellingPointRepository, SellingPointRepository>();
builder.Services.AddScoped<ISubAccountRepository, SubAccountRepository>();
builder.Services.AddScoped<ISubChannelRepository, SubChannelRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<ISubActivityTypeRepository, SubActivityTypeRepository>();
builder.Services.AddScoped<ISubActivityRepository, SubActivityRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IMechanismRepository, MechanismRepository>();
builder.Services.AddScoped<IMatrixPromoRepository, MatrixPromoRepository>();
builder.Services.AddScoped<IGroupBrandRepo, GroupBrandRepo>();
// Master End

// MAPPING Start
builder.Services.AddScoped<IDistributorSubAccountRepo, DistributorSubAccountRepo>();
builder.Services.AddScoped<IPICDNManualRepo, PICDNManualRepo>();
builder.Services.AddScoped<ISKUBlitzRepo, SKUBlitzRepo>();
builder.Services.AddScoped<ISubAccountBlitzRepo, SubAccountBlitzRepo>();
builder.Services.AddScoped<IUserSubAccountRepo, UserSubAccountRepo>();
builder.Services.AddScoped<ITaxLevelRepo, TaxLevelRepo>();
builder.Services.AddScoped<IPromoReconSubActivityRepo, PromoReconSubActivityRepo>();
builder.Services.AddScoped<IDistributorWHTRepo, DistributorWHTRepo>();

// MAPPING End

//REPORT START
builder.Services.AddScoped<IInvestmentReportRepo, InvestmentReportRepository>();
builder.Services.AddScoped<IAccrualReportRepo, AccrualReportRepository>();
builder.Services.AddScoped<IMatrixApprovalListingRepo, MatrixApprovalListingRepository>();
builder.Services.AddScoped<IPromoHistoricalMovementRepo, PromoHistoricalMovementRepository>();
builder.Services.AddScoped<ISummaryAgingApprovalRepo, SummaryAgingApprovalRepository>();
builder.Services.AddScoped<IListingDNRepo, ListingDNRepository>();
builder.Services.AddScoped<IDNDetailReportingRepo, DNDetailReportingRepository>();
builder.Services.AddScoped<IDNDisplayRepo, DNDisplayRepository>();
builder.Services.AddScoped<IListingPromoReconRepo, ListingPromoReconRepository>();
builder.Services.AddScoped<IPromoPlanningReportingRepo, PromoPlanningReportingRepository>();
builder.Services.AddScoped<IListingPromoReportingRepo, ListingPromoReportingRepository>();
builder.Services.AddScoped<ISKPValidationRepo, SKPValidationRepository>();
builder.Services.AddScoped<IDocumentCompletenessRepo, DocumentCompletenessRepository>();

//BUDGET START
builder.Services.AddScoped<IBudgetMasterRepository, BudgetMasterRepository>();
builder.Services.AddScoped<IBudgetApprovalRepository, BudgetApprovalRepository>();
builder.Services.AddScoped<IBudgetHistoryRepository, BudgetHistoryRepository>();
builder.Services.AddScoped<IBudgetConversionRateRepo, BudgetConversionRateRepository>();
//BUDGET END

//PROMO START
builder.Services.AddScoped<IPromoPlanningRepository, PromoPlanningRepository>();
builder.Services.AddScoped<IPromoApprovalRepository, PromoApprovalRepository>();
builder.Services.AddScoped<IPromoCreationRepository, PromoCreationRepository>();
builder.Services.AddScoped<IPromoSendbackRepository, PromoSendbackRepository>();
builder.Services.AddScoped<IPromoReconRepository, PromoReconRepository> ();
builder.Services.AddScoped<IPromoWorkflowRepository, PromoWorkflowRepository>();
builder.Services.AddScoped<IPromoSKPValidationRepository, PromoSKPValidationRepository>();
builder.Services.AddScoped<IPromoDisplayRepository, PromoDisplayRepository>();
builder.Services.AddScoped<IPromoClosureRepository, PromoClosureRepository>();
builder.Services.AddScoped<IPromoCancelRepository, PromoCancelRepository>();
//PROMO END

//PROMO V2 START
// SN#216 EP1 2024 - Item 1, 42 - 1. Promo Creation
builder.Services.AddScoped<IPromoCreationV2Repository, PromoCreationV2Repository>();
builder.Services.AddScoped<IPromoCalculatorRepo, PromoCalculatorRepo>();
builder.Services.AddScoped<IPromoReconV2Repository, PromoReconV2Repository>();
//PROMO V2 END

//BudgetV2
builder.Services.AddScoped<IBudgetV2Repository, BudgetV2Repository>();

// FINANCE REPORT START
#region FINANCE REPORT
builder.Services.AddScoped<IFinAccrualReportRepo, FinAccrualReportRepository>();
builder.Services.AddScoped<IFinDNDetailReportingRepo, FinDNDetailReportingRepository>();
builder.Services.AddScoped<IFinDNDisplayRepo, FinDNDisplayRepository>();
builder.Services.AddScoped<IFinDocumentCompletenessRepo, FinDocumentCompletenessRepository>();
builder.Services.AddScoped<IFinInvestmentReportRepo, FinInvestmentReportRepository>();
builder.Services.AddScoped<IFinListingDNRepo, FinListingDNRepository>();
builder.Services.AddScoped<IFinListingPromoReconRepo, FinListingPromoReconRepo>();
builder.Services.AddScoped<IFinListingPromoReportingRepo, FinListingPromoReportingRepository>();
builder.Services.AddScoped<IFinMatrixApprovalListingRepo, FinMatrixApprovalListingRepository>();
builder.Services.AddScoped<IFinPromoHistoricalMovementRepo, FinPromoHistoricalMovementRepository>();
builder.Services.AddScoped<IFinPromoPlanningReportingRepo, FinPromoPlanningReportingRepository>();
builder.Services.AddScoped<IFinSKPValidationRepo, FinSKPValidationRepository>();
builder.Services.AddScoped<IFinSummaryAgingApprovalRepo, FinSummaryAgingApprovalRepository>();
builder.Services.AddScoped<IFinPromoDisplayRepo, FinPromoDisplayRepository>();
builder.Services.AddScoped<IFinPromoApprovalAgingRepo, FinPromoApprovalAgingRepository>();
builder.Services.AddScoped<IFinPromoSubmissionReportRepo, FinPromoSubmissionRepository>();
builder.Services.AddScoped<IFinPromoApprovalReminderRepository, FinPromoApprovalReminderRepository>();
#endregion
// FINANCE REPORT END

//added by andrie, August 10 2023
builder.Services.AddScoped<IReferencesRepository, ReferencesRepository>();

// SERVICES START
builder.Services.AddScoped<IFileService, FileService>();
// SERVICES End

// add Timer Services
//added by andrie, July 11 2024
builder.Services.AddSingleton<TimerService>();
builder.Services.AddSingleton<TimerTask>();


builder.Services.AddSwaggerGen(cfg =>
{
    cfg.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v7",
            Title = "Optima-System API's "
        });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    cfg.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };
    cfg.AddSecurityRequirement(securityRequirement);
    var filePath = Path.Combine(AppContext.BaseDirectory, "V7.xml");
    cfg.IncludeXmlComments(filePath);

});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    // .AllowCredentials();

}));

ConfigurationManager configuration = builder.Configuration;
string SECRET = configuration.GetSection("AppSettings:Secret").Value ?? throw new Exception();
var secretKey = Encoding.ASCII.GetBytes(SECRET);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    x.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
    {
        // Skip the default logic.
        context.HandleResponse();
        var payload = new JObject
        {
            ["code"] = 401,
            ["error"] = true,
            ["message"] = (context.ErrorDescription == "" || context.ErrorDescription == null) ? "Please login first" : context.ErrorDescription,
            ["values"] = ""
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 401;

        return context.Response.WriteAsync(payload.ToString());
    }
    };
});

var app = builder.Build();
app.UseSwagger();

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);
// Configure the HTTP request pipeline.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Optima-System API's");
   
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseHttpMetrics();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseStaticFiles();
app.MapMetrics();

// Ensure the TimerService is created so it starts running
app.Services.GetService<TimerService>();

app.Run();
