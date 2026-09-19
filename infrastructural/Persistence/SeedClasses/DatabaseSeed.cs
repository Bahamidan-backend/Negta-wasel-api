namespace Persistence_Layer.Persistence.SeedClasses;

public static class DatabaseSeed
{
    public static async Task SeedDatabaseAsync(ApplicationDbContext context, UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        Role[] rolesToAdd =
        [
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN",
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Owner",
                NormalizedName = "OWNER",
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "User",
                NormalizedName = "USER",
            }
        ];
        foreach (var role in rolesToAdd)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                await roleManager.CreateAsync(role);
            }
        }

        await context.SaveChangesAsync();

        var names = new ArrayOfNames();
        if (!context.Categories.Any())
        {
            // قاموس لربط الفئة الرئيسية بأيقونتها المناسبة من الملف
            var categoryIcons = new Dictionary<string, string>
            {
                { "الرياضة واللياقة", "gym_fitness" },
                { "شركات ومؤسسات", "architecture_firm" },
                { "حدائق وأماكن مفتوحة", "landscape" },
                { "مساجد ودور عبادة", "funeral_service" }, // Church/Place of worship theme
                { "مرافق حكومية وعامة", "bank" },
                { "نقل وسفر", "shipping_logistics" },
                { "تعليم", "school" },
                { "ترفيه وفنون", "theme_park" },
                { "خدمات عامة", "repair_service" },
                { "تسوق وتجزئة", "store" },
                { "خدمات طبية وصحية", "hospital" },
                { "فنادق وإقامة", "hotel_stay" },
                { "مطاعم وكافيهات", "restaurant" }
            };

            foreach (var placeName in names.Names)
            {
                if (context.Categories.Any(x => x.CategoryName == placeName))
                {
                    continue;
                }

                categoryIcons.TryGetValue(placeName, out var icon);

                var category = new Category
                {
                    CategoryName = placeName,
                    CategoryIcon = icon ?? "store" // أيقونة افتراضية في حال لم يجد مطابقة
                };

                await context.Categories.AddAsync(category);
            }

            await context.SaveChangesAsync();
        }

        // sub-categories seed
        string[][] categoriesWithSubcategories = new string[][]
        {
            ["ملاعب كرة قدم", "صالات رياضية (Gym)", "مسابح", "مراكز فنون قتالية", "مضامير جري"],
            ["شركات تقنية", "بنوك ومصارف", "مصانع", "شركات استشارات", "مكاتب إدارية"],
            ["حدائق عامة", "حدائق نباتية", "شواطئ", "مسارات مشي جبلية", "كورنيش"],
            ["مساجد", "كنائس", "معابد", "مصليات", "مقابر"],
            ["بلديات", "مراكز شرطة", "مراكز دفاع مدني", "مكاتب بريد", "مكتبات عامة"],
            ["مطارات", "محطات حافلات", "محطات قطارات", "مكاتب تأجير سيارات", "وكالات سفر"],
            ["مدارس", "جامعات", "مراكز تدريب", "رياض أطفال", "معاهد لغات"],
            ["دور سينما", "معارض فنية", "مدن ملاهي", "مسارح", "متاحف"],
            ["مغاسل سيارات", "مغاسل ملابس", "صالونات حلاقة", "صالونات تجميل", "ورش صيانة"],
            ["مولات تجارية", "سوبر ماركت", "محلات ملابس", "محلات إلكترونيات", "صيدليات"],
            ["مستشفيات", "عيادات", "عيادات أسنان", "مختبرات طبية", "مراكز علاج طبيعي"],
            ["فنادق", "شقق فندقية", "منتجعات", "نُزل", "موتيلات"],
            ["مطاعم وجبات سريعة", "مطاعم عائلية", "كافيهات", "مخابز", "مطاعم شعبية"]
        };

        // قاموس مخصص لربط كل تصنيف فرعي بالأيقونة المطابقة له من ملف الـ Dart
        var subCategoryIcons = new Dictionary<string, string>
        {
            // الرياضة واللياقة
            { "ملاعب كرة قدم", "soccer_academy" },
            { "صالات رياضية (Gym)", "gym_fitness" },
            { "مسابح", "swimming_pool" },
            { "مراكز فنون قتالية", "martial_arts" },
            { "مضامير جري", "shoe_store" }, // الأقرب لـ Running/Walk

            // شركات ومؤسسات
            { "شركات تقنية", "software_house" },
            { "بنوك ومصارف", "bank" },
            { "مصانع", "construction" },
            { "شركات استشارات", "consultancy" },
            { "مكاتب إدارية", "coworking_space" },

            // حدائق وأماكن مفتوحة
            { "حدائق عامة", "picnic_spot" },
            { "حدائق نباتية", "organic_shop" },
            { "شواطئ", "beach_resort" },
            { "مسارات مشي جبلية", "camping" },
            { "كورنيش", "marina" },

            // مساجد ودور عبادة
            { "مساجد", "funeral_service" }, // يمثل رمز العبادة العام المتاح بالملف
            { "كنائس", "funeral_service" },
            { "معابد", "funeral_service" },
            { "مصليات", "funeral_service" },
            { "مقابر", "funeral_service" },

            // مرافق حكومية وعامة
            { "بلديات", "architecture_firm" },
            { "مراكز شرطة", "insurance" }, // يمثل الأمان/الحماية
            { "مراكز دفاع مدني", "emergency_care" },
            { "مكاتب بريد", "courier_service" },
            { "مكتبات عامة", "library" },

            // نقل وسفر
            { "مطارات", "travel_agency" }, // يحتوي أيقونة الطائرة flight
            { "محطات حافلات", "food_truck" }, // الأقرب للنقل البري بالموجود
            { "محطات قطارات", "courier_service" },
            { "مكاتب تأجير سيارات", "rental_cars" },
            { "وكالات سفر", "travel_agency" },

            // تعليم
            { "مدارس", "school" },
            { "جامعات", "university" },
            { "مراكز تدريب", "training_center" },
            { "رياض أطفال", "baby_store" }, // يحتوي أيقونة عربة الأطفال stroller
            { "معاهد لغات", "language_school" },

            // ترفيه وفنون
            { "دور سينما", "cinema" },
            { "معارض فنية", "art_gallery" },
            { "مدن ملاهي", "theme_park" },
            { "مسارح", "theater" },
            { "متاحف", "museum" },

            // خدمات عامة
            { "مغاسل سيارات", "car_wash" },
            { "مغاسل ملابس", "laundry_service" },
            { "صالونات حلاقة", "barber_shop" },
            { "صالونات تجميل", "beauty_salon" },
            { "ورش صيانة", "auto_repair" },

            // تسوق وتجزئة
            { "مولات تجارية", "mall" },
            { "سوبر ماركت", "shopping_cart" },
            { "محلات ملابس", "boutique" },
            { "محلات إلكترونيات", "electronics" },
            { "صيدليات", "pharmacy" },

            // خدمات طبية وصحية
            { "مستشفيات", "hospital" },
            { "عيادات", "medical_clinic" },
            { "عيادات أسنان", "dental_clinic" },
            { "مختبرات طبية", "laboratory" },
            { "مراكز علاج طبيعي", "physical_therapy" },

            // فنادق وإقامة
            { "فنادق", "hotel_stay" },
            { "شقق فندقية", "hotel_stay" },
            { "منتجعات", "beach_resort" },
            { "نُزل", "hotel_stay" },
            { "موتيلات", "hotel_stay" },

            // مطاعم وكافيهات
            { "مطاعم وجبات سريعة", "fast_food" },
            { "مطاعم عائلية", "fine_dining" },
            { "كافيهات", "cafe" },
            { "مخابز", "bakery" },
            { "مطاعم شعبية", "restaurant" }
        };

        if (!context.SubCategories.Any())
        {
            for (int i = 0; i < categoriesWithSubcategories.Length; i++)
            {
                string currentCategoryName = i switch
                {
                    0 => "الرياضة واللياقة",
                    1 => "شركات ومؤسسات",
                    2 => "حدائق وأماكن مفتوحة",
                    3 => "مساجد ودور عبادة",
                    4 => "مرافق حكومية وعامة",
                    5 => "نقل وسفر",
                    6 => "تعليم",
                    7 => "ترفيه وفنون",
                    8 => "خدمات عامة",
                    9 => "تسوق وتجزئة",
                    10 => "خدمات طبية وصحية",
                    11 => "فنادق وإقامة",
                    12 => "مطاعم وكافيهات",
                    _ => ""
                };

                var category = context.Categories.Single(x => x.CategoryName == currentCategoryName);

                for (int j = 0; j < categoriesWithSubcategories[i].Length; j++)
                {
                    string subName = categoriesWithSubcategories[i][j];
                    subCategoryIcons.TryGetValue(subName, out var icon);

                    context.SubCategories.Add(new SubCategory
                    {
                        Name = subName,
                        supCategoryIcon = icon ?? "store", // أيقونة افتراضية في حال لم يجد مطابقة
                        CategoryId = category.CategoryId,
                        Category = category
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}