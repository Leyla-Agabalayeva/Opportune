using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Opportune.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    slug = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    name_az = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_en = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    slug = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    name_az = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_en = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_skills", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    country = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    bio = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    avatar_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    preferred_language = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.CheckConstraint("ck_users_preferred_language", "preferred_language IN ('az', 'en', 'ru')");
                });

            migrationBuilder.CreateTable(
                name: "role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claims_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    website_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    country = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: true),
                    contact_email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizations", x => x.id);
                    table.ForeignKey(
                        name: "fk_organizations_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recommendation_caches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    result_json = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recommendation_caches", x => x.id);
                    table.ForeignKey(
                        name: "fk_recommendation_caches_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token_hash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    replaced_by_token_hash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true),
                    country = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscriptions", x => x.id);
                    table.CheckConstraint("ck_subscriptions_has_filter", "category_id IS NOT NULL OR country IS NOT NULL");
                    table.ForeignKey(
                        name: "fk_subscriptions_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_subscriptions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claims_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_interests",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_interests", x => new { x.user_id, x.category_id });
                    table.ForeignKey(
                        name: "fk_user_interests_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_interests_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_languages",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    level = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_languages", x => new { x.user_id, x.language_code });
                    table.ForeignKey(
                        name: "fk_user_languages_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_user_logins_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_skills",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    skill_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_skills", x => new { x.user_id, x.skill_id });
                    table.ForeignKey(
                        name: "fk_user_skills_skills_skill_id",
                        column: x => x.skill_id,
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_skills_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_user_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "opportunities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    original_language = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    scope = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    country = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_online = table.Column<bool>(type: "boolean", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    application_deadline = table.Column<DateOnly>(type: "date", nullable: true),
                    duration_days = table.Column<int>(type: "integer", nullable: true),
                    min_age = table.Column<int>(type: "integer", nullable: true),
                    max_age = table.Column<int>(type: "integer", nullable: true),
                    accommodation_covered = table.Column<bool>(type: "boolean", nullable: false),
                    meals_covered = table.Column<bool>(type: "boolean", nullable: false),
                    travel_covered = table.Column<bool>(type: "boolean", nullable: false),
                    participation_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    currency = table.Column<string>(type: "character(3)", fixedLength: true, maxLength: 3, nullable: true),
                    apply_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    verification_level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    last_verified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    source_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    external_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    rejection_reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    search_vector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true, computedColumnSql: "to_tsvector('simple', coalesce(title, '') || ' ' || coalesce(description, ''))", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_opportunities", x => x.id);
                    table.CheckConstraint("ck_opportunities_age_range", "min_age IS NULL OR max_age IS NULL OR min_age <= max_age");
                    table.CheckConstraint("ck_opportunities_date_range", "start_date IS NULL OR end_date IS NULL OR end_date >= start_date");
                    table.CheckConstraint("ck_opportunities_fee_currency", "participation_fee >= 0 AND (participation_fee = 0 OR currency IS NOT NULL)");
                    table.CheckConstraint("ck_opportunities_location", "is_online OR country IS NOT NULL");
                    table.CheckConstraint("ck_opportunities_original_language", "original_language IN ('az', 'en', 'ru')");
                    table.ForeignKey(
                        name: "fk_opportunities_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_opportunities_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_opportunities_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "application_trackings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    opportunity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    applied_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_trackings", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_trackings_opportunities_opportunity_id",
                        column: x => x.opportunity_id,
                        principalTable: "opportunities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_application_trackings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    opportunity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_opportunities_opportunity_id",
                        column: x => x.opportunity_id,
                        principalTable: "opportunities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_notifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "opportunity_required_languages",
                columns: table => new
                {
                    opportunity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    min_level = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_opportunity_required_languages", x => new { x.opportunity_id, x.language_code });
                    table.ForeignKey(
                        name: "fk_opportunity_required_languages_opportunities_opportunity_id",
                        column: x => x.opportunity_id,
                        principalTable: "opportunities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "opportunity_skills",
                columns: table => new
                {
                    opportunity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    skill_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_opportunity_skills", x => new { x.opportunity_id, x.skill_id });
                    table.ForeignKey(
                        name: "fk_opportunity_skills_opportunities_opportunity_id",
                        column: x => x.opportunity_id,
                        principalTable: "opportunities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_opportunity_skills_skills_skill_id",
                        column: x => x.skill_id,
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "opportunity_translations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    opportunity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    source_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    translated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_opportunity_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_opportunity_translations_opportunities_opportunity_id",
                        column: x => x.opportunity_id,
                        principalTable: "opportunities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "participations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    opportunity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    certificate_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_participations", x => x.id);
                    table.CheckConstraint("ck_participations_date_range", "start_date IS NULL OR end_date IS NULL OR end_date >= start_date");
                    table.ForeignKey(
                        name: "fk_participations_opportunities_opportunity_id",
                        column: x => x.opportunity_id,
                        principalTable: "opportunities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_participations_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "saved_opportunities",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    opportunity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    saved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_saved_opportunities", x => new { x.user_id, x.opportunity_id });
                    table.ForeignKey(
                        name: "fk_saved_opportunities_opportunities_opportunity_id",
                        column: x => x.opportunity_id,
                        principalTable: "opportunities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_saved_opportunities_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    participation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    is_hidden = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.id);
                    table.CheckConstraint("ck_reviews_rating", "rating BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "fk_reviews_participations_participation_id",
                        column: x => x.participation_id,
                        principalTable: "participations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name_az", "name_en", "name_ru", "slug" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), "Ətraf mühit", "Environment", "Экология", "environment" },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), "Təhsil", "Education", "Образование", "education" },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), "Mədəniyyət və incəsənət", "Culture and arts", "Культура и искусство", "culture-arts" },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), "Sosial dəstək", "Social care", "Социальная помощь", "social-care" },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), "Sağlamlıq", "Health", "Здоровье", "health" },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), "Gənclər", "Youth", "Молодёжь", "youth" },
                    { new Guid("c0000000-0000-0000-0000-000000000007"), "İdman", "Sports", "Спорт", "sports" },
                    { new Guid("c0000000-0000-0000-0000-000000000008"), "Tədbirlər və konfranslar", "Events and conferences", "Мероприятия и конференции", "events" },
                    { new Guid("c0000000-0000-0000-0000-000000000009"), "Heyvanların müdafiəsi", "Animal welfare", "Защита животных", "animals" },
                    { new Guid("c0000000-0000-0000-0000-000000000010"), "Humanitar yardım", "Humanitarian aid", "Гуманитарная помощь", "humanitarian" },
                    { new Guid("c0000000-0000-0000-0000-000000000011"), "Texnologiya", "Technology", "Технологии", "technology" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "role-seed-1", "Volunteer", "VOLUNTEER" },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "role-seed-2", "Organizer", "ORGANIZER" },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), "role-seed-3", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "skills",
                columns: new[] { "id", "name_az", "name_en", "name_ru", "slug" },
                values: new object[,]
                {
                    { new Guid("d0000000-0000-0000-0000-000000000001"), "Ünsiyyət", "Communication", "Коммуникация", "communication" },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), "Komanda işi", "Teamwork", "Работа в команде", "teamwork" },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), "Liderlik", "Leadership", "Лидерство", "leadership" },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), "Tədbir təşkili", "Event organization", "Организация мероприятий", "event-organization" },
                    { new Guid("d0000000-0000-0000-0000-000000000005"), "Tədris", "Teaching", "Преподавание", "teaching" },
                    { new Guid("d0000000-0000-0000-0000-000000000006"), "Natiqlik", "Public speaking", "Публичные выступления", "public-speaking" },
                    { new Guid("d0000000-0000-0000-0000-000000000007"), "Fotoqrafiya", "Photography", "Фотография", "photography" },
                    { new Guid("d0000000-0000-0000-0000-000000000008"), "Video istehsalı", "Video production", "Видеопроизводство", "video-production" },
                    { new Guid("d0000000-0000-0000-0000-000000000009"), "Qrafik dizayn", "Graphic design", "Графический дизайн", "graphic-design" },
                    { new Guid("d0000000-0000-0000-0000-000000000010"), "Sosial media", "Social media", "Социальные сети", "social-media" },
                    { new Guid("d0000000-0000-0000-0000-000000000011"), "Yazı və redaktə", "Writing and editing", "Написание и редактура", "writing" },
                    { new Guid("d0000000-0000-0000-0000-000000000012"), "Tərcümə", "Translation", "Перевод", "translation" },
                    { new Guid("d0000000-0000-0000-0000-000000000013"), "İlkin tibbi yardım", "First aid", "Первая помощь", "first-aid" },
                    { new Guid("d0000000-0000-0000-0000-000000000014"), "Vəsait toplanması", "Fundraising", "Сбор средств", "fundraising" },
                    { new Guid("d0000000-0000-0000-0000-000000000015"), "Proqramlaşdırma", "Programming", "Программирование", "programming" },
                    { new Guid("d0000000-0000-0000-0000-000000000016"), "Məlumat təhlili", "Data analysis", "Анализ данных", "data-analysis" },
                    { new Guid("d0000000-0000-0000-0000-000000000017"), "Layihə idarəçiliyi", "Project management", "Управление проектами", "project-management" },
                    { new Guid("d0000000-0000-0000-0000-000000000018"), "Tədqiqat", "Research", "Исследования", "research" },
                    { new Guid("d0000000-0000-0000-0000-000000000019"), "Yemək hazırlama", "Cooking", "Приготовление пищи", "cooking" },
                    { new Guid("d0000000-0000-0000-0000-000000000020"), "Uşaqlarla iş", "Working with children", "Работа с детьми", "working-with-children" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_application_trackings_opportunity_id",
                table: "application_trackings",
                column: "opportunity_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_trackings_user_id_opportunity_id",
                table: "application_trackings",
                columns: new[] { "user_id", "opportunity_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_categories_slug",
                table: "categories",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_notifications_opportunity_id",
                table: "notifications",
                column: "opportunity_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id_is_read_created_at",
                table: "notifications",
                columns: new[] { "user_id", "is_read", "created_at" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_category_id",
                table: "opportunities",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_country",
                table: "opportunities",
                column: "country");

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_created_by_user_id",
                table: "opportunities",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_organization_id",
                table: "opportunities",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_scope",
                table: "opportunities",
                column: "scope");

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_search_vector",
                table: "opportunities",
                column: "search_vector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_source_external_id",
                table: "opportunities",
                columns: new[] { "source", "external_id" },
                unique: true,
                filter: "external_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_opportunities_status_application_deadline",
                table: "opportunities",
                columns: new[] { "status", "application_deadline" });

            migrationBuilder.CreateIndex(
                name: "ix_opportunity_skills_skill_id",
                table: "opportunity_skills",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_opportunity_translations_opportunity_id_language_code",
                table: "opportunity_translations",
                columns: new[] { "opportunity_id", "language_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_organizations_owner_user_id",
                table: "organizations",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_organizations_slug",
                table: "organizations",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_participations_opportunity_id",
                table: "participations",
                column: "opportunity_id");

            migrationBuilder.CreateIndex(
                name: "ix_participations_user_id_opportunity_id",
                table: "participations",
                columns: new[] { "user_id", "opportunity_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recommendation_caches_user_id_profile_hash",
                table: "recommendation_caches",
                columns: new[] { "user_id", "profile_hash" });

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_token_hash",
                table: "refresh_tokens",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_participation_id",
                table: "reviews",
                column: "participation_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_role_claims_role_id",
                table: "role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_saved_opportunities_opportunity_id",
                table: "saved_opportunities",
                column: "opportunity_id");

            migrationBuilder.CreateIndex(
                name: "ix_skills_slug",
                table: "skills",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subscriptions_category_id",
                table: "subscriptions",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscriptions_user_id_category_id_country",
                table: "subscriptions",
                columns: new[] { "user_id", "category_id", "country" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "ix_user_claims_user_id",
                table: "user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_interests_category_id",
                table: "user_interests",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_user_id",
                table: "user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_skills_skill_id",
                table: "user_skills",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "users",
                column: "normalized_user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_trackings");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "opportunity_required_languages");

            migrationBuilder.DropTable(
                name: "opportunity_skills");

            migrationBuilder.DropTable(
                name: "opportunity_translations");

            migrationBuilder.DropTable(
                name: "recommendation_caches");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "role_claims");

            migrationBuilder.DropTable(
                name: "saved_opportunities");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "user_claims");

            migrationBuilder.DropTable(
                name: "user_interests");

            migrationBuilder.DropTable(
                name: "user_languages");

            migrationBuilder.DropTable(
                name: "user_logins");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "user_skills");

            migrationBuilder.DropTable(
                name: "user_tokens");

            migrationBuilder.DropTable(
                name: "participations");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "opportunities");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "organizations");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
