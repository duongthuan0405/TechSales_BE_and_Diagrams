using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechSalesManagement.Application.Common.Configurations;
using TechSalesManagement.Application.HelperServices;
using TechSalesManagement.Application.Interfaces;
using TechSalesManagement.Application.Services.Implementations;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Infrastructure.HelperServices;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Infrastructure.Repositories;
using TechSalesManagement.Application.VoucherStrategies;


namespace TechSalesManagement.Presentation_WebAPI.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OtpCO>(configuration.GetSection("OTP"));
        services.Configure<MailSettingsCO>(configuration.GetSection("MAIL"));
        services.Configure<FrontendCO>(configuration.GetSection("FE"));
        services.Configure<JwtCO>(configuration.GetSection("JWT"));
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IShippingAddressService, ShippingAddressService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IReviewService, ReviewService>();

        // Discount Strategies
        services.AddScoped<IDiscountStrategy, FixedDiscountStrategy>();
        services.AddScoped<IDiscountStrategy, PercentDiscountStrategy>();
        services.AddScoped<IDiscountStrategyFactory, DiscountStrategyFactory>();

        return services;
    }

    public static IServiceCollection AddExternalAndHelperServices(this IServiceCollection services)
    {
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IShippingAddressRepository, ShippingAddressRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        return services;
    }
}
