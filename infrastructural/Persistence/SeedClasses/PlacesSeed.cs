namespace Persistence_Layer.Persistence.SeedClasses;

public static class PlacesSeed
{
    public static async Task SeedDatabaseAsync(ApplicationDbContext context, UserManager<User> userManager)
    {
        if (!context.Places.Any())
        {
            var owner = userManager.Users.Where(x => x.Role.Name!.ToLower() == "user");
            var user = userManager.Users.Where(x => x.Role.Name!.ToLower() == "owner");
            var request = new Request
            {
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                State = RequestStates.Accepted,
                UserId = owner.First().Id,
            };
            var location = new Location
            {
                Latitude = 5.051651654,
                Longitude = 14.1561651,
                DirectorateId = 1,
                DistrictId = 4,
                NearestLandmark = "جامعة حضرموت المساكن",
            };
            var newPlace = new Place
            {
                PlaceName = "بيتزا هاوس",
                Description = "مطعم لتقديم مختلف انواع البيتزا في المكلا",
                OpeningTime = new TimeOnly(8, 00), // يمثل 24 ساعدة من الساعة 8 صباحا وحتى 11 مساءً.
                ClosingTime = new TimeOnly(23, 00),
                Email = "qwerty@gmail.com",
                CreatedAt = DateTime.UtcNow.AddYears(-5).AddMonths(-5).AddDays(-27),
                UnavailableAt = [WeekDays.الجمعة],
                Request = request,
                SubCategoryId = 62,
                UserId = owner.First().Id,
                Location = location,
                Rates =
                {
                    new Review
                    {
                        CreatedAt = DateTime.UtcNow.AddHours(-1),
                        User = user.First(), UserId = user.First().Id,
                        Note = "المكان رائع ويستحق الزيارة المتكررة.", RateValue = 5
                    },
                    new Review
                    {
                        CreatedAt = DateTime.UtcNow.AddHours(-2),
                        User = user.First(),
                        UserId = user.ElementAt(2).Id,
                        Note = "جيد يحتاج إلى مراعاة الاسعار و فتح فرع جديد بسبب الزحمة الزادئة", RateValue = 3
                    },
                    new Review
                    {
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        User = user.First(), UserId = user.ElementAt(3).Id,
                        Note = "ممتاز، ولكن ليس الافضل", RateValue = 4
                    }
                },
                CommericalRegisteration = new CommericalRegisteration()
                {
                    CrNumber = "a11b011wecc56t",
                    EntityName = "بيتزا هاوس",
                    ExpirationDate = DateTime.UtcNow.AddYears(5).AddDays(45),
                    ImagePath = "",
                    User = owner.First(),
                    UserId = owner.First().Id,
                },
                Images = new List<PlaceImage>()
                {
                    new()
                    {
                        ImageUrl = "https://connectionpointapp.tryasp.net/images/places/Pizza'sHouse/unnamed.png",
                    },
                    new()
                    {
                        ImageUrl = "https://connectionpointapp.tryasp.net/images/places/Pizza'sHouse/pizza.png",
                    }
                },
                Phones = { new PhoneNumber() { Number = "000000000" } },
            };
            context.Places.Add(newPlace);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedFakeData(ApplicationDbContext context, UserManager<User> userManager)
    {
        if (await context.Places.CountAsync() <= 1)
        {
            var ownersList = userManager.Users.Where(x => x.Role.Name!.ToLower() == "user").ToList();
            var usersList = userManager.Users.Where(x => x.Role.Name!.ToLower() == "owner").ToList();

            var defaultOwner = ownersList.FirstOrDefault();
            var defaultUser1 = usersList.ElementAtOrDefault(0);
            var defaultUser2 = usersList.ElementAtOrDefault(1) ?? defaultUser1;
            var defaultUser3 = usersList.ElementAtOrDefault(2) ?? defaultUser1;

            if (defaultOwner == null || defaultUser1 == null)
                return;

            var directorates = await context.Directorates.ToListAsync();
            var districts = await context.Districts.ToListAsync();

            var getGeoIds = new Func<string, (int dirId, int distId)>((address) =>
            {
                int dirId = directorates.FirstOrDefault()?.Id ?? 1;
                int distId = districts.FirstOrDefault()?.Id ?? 1;

                if (address.Contains("المكلا"))
                {
                    var dir = directorates.FirstOrDefault(d => d.Name == "المكلا");
                    if (dir != null)
                    {
                        dirId = dir.Id;
                        var dist = districts.FirstOrDefault(d => d.DirectorateId == dir.Id);
                        if (dist != null) distId = dist.Id;
                    }
                }
                if (address.Contains("الديس"))
                {
                    var dir = directorates.FirstOrDefault(d => d.Name == "المكلا");
                    if (dir != null)
                    {
                        dirId = dir.Id;
                        var dist = districts.FirstOrDefault(d => d.Name == "الديس" && d.DirectorateId == dir.Id);
                        if (dist != null) distId = dist.Id;
                    }
                }
                else if (address.Contains("فوة") || address.Contains("فوه"))
                {
                    var dir = directorates.FirstOrDefault(d => d.Name == "المكلا");
                    if (dir != null)
                    {
                        dirId = dir.Id;
                        var dist = districts.FirstOrDefault(d => (d.Name == "فوه" || d.Name == "فوه القديمة") && d.DirectorateId == dir.Id);
                        if (dist != null) distId = dist.Id;
                    }
                }
                else if (address.Contains("الشحر"))
                {
                    var dir = directorates.FirstOrDefault(d => d.Name == "الشحر");
                    if (dir != null)
                    {
                        dirId = dir.Id;
                        var dist = districts.FirstOrDefault(d => d.DirectorateId == dir.Id);
                        if (dist != null) distId = dist.Id;
                    }
                }
                else if (address.Contains("قصيعر") || address.Contains("الريدة"))
                {
                    var dir = directorates.FirstOrDefault(d => d.Name == "الريدة وقصيعر");
                    if (dir != null)
                    {
                        dirId = dir.Id;
                        var dist = districts.FirstOrDefault(d => d.DirectorateId == dir.Id);
                        if (dist != null) distId = dist.Id;
                    }
                }

                return (dirId, distId);
            });

            var subCategories = await context.SubCategories.ToListAsync();
            var cafeSub = subCategories.FirstOrDefault(s => s.Name == "كافيهات")?.SubCategoryId ?? 62;
            var restaurantSub = subCategories.FirstOrDefault(s => s.Name == "مطاعم شعبية" || s.Name == "مطاعم عائلية")?.SubCategoryId ?? 62;
            var carWashSub = subCategories.FirstOrDefault(s => s.Name == "مغاسل سيارات")?.SubCategoryId ?? 62;

            var placesToAdd = new List<Place>
            {
                // 1. Lo10 cafe
                new Place
                {
                    PlaceName = "Lo10 cafe",
                    Description = "كافيه مميز يقدم تشكيلة رائعة من القهوة والشاي والأجواء العصرية الهادئة.",
                    Email = "lo10.cafe@example.com",
                    OpeningTime = new TimeOnly(16, 0),
                    ClosingTime = new TimeOnly(23, 0),
                    CreatedAt = DateTime.UtcNow.AddMonths(-3),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-10), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = cafeSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 14.5307528,
                        Longitude = 49.1221657,
                        DirectorateId = getGeoIds("G4JC+8V3، المكلا، اليمن").dirId,
                        DistrictId = getGeoIds("G4JC+8V3، المكلا، اليمن").distId,
                        NearestLandmark = "G4JC+8V3، المكلا",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-2), User = defaultUser1, UserId = defaultUser1.Id, Note = "قهوة رائعة وأجواء مريحة جداً للعمل والدراسة.", RateValue = 5 },
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-5), User = defaultUser2, UserId = defaultUser2.Id, Note = "مكان جميل وتشكيلة شاي مميزة.", RateValue = 4 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-LO10CAFE-01",
                        EntityName = "Lo10 cafe",
                        ExpirationDate = DateTime.UtcNow.AddYears(3),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>
                    {
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAGVRCcoGOyI4-K3n9rgYDaQYNDYKch9qkZUGgJGM_4eLRQ44WVNMq0G3mvSvNkJ6ypoP_u9LKWN2PNSeScpmLYd1nNPTH_IPK9POJd-tEWx-mW-oZG_4cZSazzefc6mVZaKzr8d=w1920-h1080-k-no" },
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAERTMB9CyGsAOuyCPCo9Bs8ZT_oaxBRoiteTkmsVjiqv6G-9-cIzu2x-LPJj-bMD9CQtguNcRkBG66Tay9Mr7jxNQSHGzN67OeCGA8FuHkTln7vGcJfVFEePJ09uo9h2iukZdNKWw=w1920-h1080-k-no" }
                    },
                    Phones = new List<PhoneNumber>()
                },

                // 2. ففتي فايف كافيه
                new Place
                {
                    PlaceName = "ففتي فايف كافيه",
                    Description = "كافيه متميز بمكان مول يقدم أشهى أنواع القهوة والمشروبات الباردة والساخنة.",
                    Email = "fiftyfive.cafe@example.com",
                    OpeningTime = new TimeOnly(16, 0),
                    ClosingTime = new TimeOnly(23, 0),
                    CreatedAt = DateTime.UtcNow.AddMonths(-2),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-8), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = cafeSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 14.9900701,
                        Longitude = 49.9366899,
                        DirectorateId = getGeoIds("الديس/جولة الكتاب/ مكان مول، اليمن").dirId,
                        DistrictId = getGeoIds("الديس/جولة الكتاب/ مكان مول، اليمن").distId,
                        NearestLandmark = "مكان مول، الديس",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-3), User = defaultUser1, UserId = defaultUser1.Id, Note = "كافيه رائع وموقع مميز داخل مكان مول.", RateValue = 5 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-55CAFE-02",
                        EntityName = "ففتي فايف كافيه",
                        ExpirationDate = DateTime.UtcNow.AddYears(3),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>(),
                    Phones = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "770379721" }
                    }
                },

                // 3. المقهاية
                new Place
                {
                    PlaceName = "المقهاية",
                    Description = "مكان رائع وأجواء خفيفة ومناسبة للمجموعات والشباب للاستمتاع بالقهوة.",
                    Email = "almeqhayah@example.com",
                    OpeningTime = new TimeOnly(16, 0),
                    ClosingTime = new TimeOnly(23, 30),
                    CreatedAt = DateTime.UtcNow.AddMonths(-4),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-12), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = cafeSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 14.4834398,
                        Longitude = 49.0523283,
                        DirectorateId = getGeoIds("F3M2+9W، فوة، اليمن").dirId,
                        DistrictId = getGeoIds("F3M2+9W، فوة، اليمن").distId,
                        NearestLandmark = "F3M2+9W، فوة",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-1), User = defaultUser1, UserId = defaultUser1.Id, Note = "الموقع ممتاز والخدمة جيدة جداً.", RateValue = 5 },
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-4), User = defaultUser2, UserId = defaultUser2.Id, Note = "مكان مميز وهادئ.", RateValue = 5 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-MEQHA-03",
                        EntityName = "المقهاية",
                        ExpirationDate = DateTime.UtcNow.AddYears(2),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>
                    {
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAGDUMIoh26OjWdJiZWM2YuL6OXEhdH5ufbpnESlB3Q4bZPt0pwCBqfLyL2hrl1aETAoZXoCMrQB-h2aLM3tC_kjmF9ly_jaiCg3Q0127k9u7T9QuXZvN3UArgg1KbClR56fmxMLlN2ZndY=w1920-h1080-k-no" },
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAF2cQ-7XCHZSWx8vprmYJYNxPVJr_n7_ZDYSufXlucKCOY69E4AvV1IgBISw8ziU1DmUgtQDDzWp4uNd5IuBlvYGRCBfRstt3pUQOf3XpqIShGfWAhzugqZsCqk7EnIx6n0F4EC5g5MqCoE=w1920-h1080-k-no" }
                    },
                    Phones = new List<PhoneNumber>()
                },

                // 4. مغسال الجرو للسيارات
                new Place
                {
                    PlaceName = "مغسال الجرو للسيارات",
                    Description = "خدمة غسيل وتلميع السيارات بأحدث المعدات مع خدمة تغسيل المكيفات.",
                    Email = "aljarw.wash@example.com",
                    OpeningTime = new TimeOnly(8, 0),
                    ClosingTime = new TimeOnly(18, 0),
                    CreatedAt = DateTime.UtcNow.AddMonths(-6),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-20), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = carWashSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 14.944081,
                        Longitude = 50.3423656,
                        DirectorateId = getGeoIds("W8VR+JWQ, الشارع العام, قصيعر، اليمن").dirId,
                        DistrictId = getGeoIds("W8VR+JWQ, الشارع العام, قصيعر، اليمن").distId,
                        NearestLandmark = "محطة الجرو للمحروقات، قصيعر",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-10), User = defaultUser1, UserId = defaultUser1.Id, Note = "غسيل نظيف وسريع، ويوجد خدمة تغسيل المكيفات.", RateValue = 5 },
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-15), User = defaultUser2, UserId = defaultUser2.Id, Note = "خدمة ممتازة وأنصح بالتعامل معهم.", RateValue = 4 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-JARW-04",
                        EntityName = "مغسال الجرو للسيارات",
                        ExpirationDate = DateTime.UtcNow.AddYears(4),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>
                    {
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAGafwsczrHnqz-mD6Esl0NkS7ihEN8YEnNzqn7xOgeQ04WTsnd-WugNYcVmzAr3eWCsBaJqWo_nkO47iHpFDqXGnxX8f-_xc7psGZ2V5xPClL-7GG83TBLsZYWpTFQtRz9COMY=w1920-h1080-k-no" },
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAGlSoNAH7396KV4VIorey5OTttpbp1flrrbY5DDWSzrpbho5EQadAV3q9c1dUkJXyHH4UQtEt618q8_FITGCo1BgcMAAbnTnv1Oe1FZIOBtN2f9BJ-LxSvlBIiet7DhjctDTrG7=w1920-h1080-k-no" }
                    },
                    Phones = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "775472681" }
                    }
                },

                // 5. بوفية مدرسة الشهيد باعباد
                new Place
                {
                    PlaceName = "بوفية مدرسة الشهيد باعباد",
                    Description = "بوفية تقدم وجبات خفيفة ومشروبات ووجبات إفطار للطلاب والزوار.",
                    Email = "baabad.buffet@example.com",
                    OpeningTime = new TimeOnly(7, 30),
                    ClosingTime = new TimeOnly(14, 0),
                    CreatedAt = DateTime.UtcNow.AddMonths(-5),
                    UnavailableAt = [WeekDays.الجمعة, WeekDays.السبت],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-15), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = cafeSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 14.9382226,
                        Longitude = 50.3384023,
                        DirectorateId = getGeoIds("W8QQ+78، قصيعر، اليمن").dirId,
                        DistrictId = getGeoIds("W8QQ+78، قصيعر، اليمن").distId,
                        NearestLandmark = "مدرسة الشهيد باعباد، قصيعر",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-20), User = defaultUser3, UserId = defaultUser3.Id, Note = "خدمة جيدة ومناسبة للطلاب.", RateValue = 4 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-BAABAD-05",
                        EntityName = "بوفية مدرسة الشهيد باعباد",
                        ExpirationDate = DateTime.UtcNow.AddYears(1),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>(),
                    Phones = new List<PhoneNumber>()
                },

                // 6. مجمع الشرق السياحي بحضاتهم
                new Place
                {
                    PlaceName = "مجمع الشرق السياحي بحضاتهم",
                    Description = "مطعم ومجمع سياحي يقدم وجبات الغداء والعشاء المتنوعة مع خدمات التزويد بالطعام.",
                    Email = "alsharq.complex@example.com",
                    OpeningTime = new TimeOnly(5, 30),
                    ClosingTime = new TimeOnly(23, 59),
                    CreatedAt = DateTime.UtcNow.AddMonths(-8),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-30), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = restaurantSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 15.0941625,
                        Longitude = 50.7440539,
                        DirectorateId = getGeoIds("N 4, اليمن").dirId,
                        DistrictId = getGeoIds("N 4, اليمن").distId,
                        NearestLandmark = "منطقة حضاتهم",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-2), User = defaultUser1, UserId = defaultUser1.Id, Note = "من أفضل المجمعات السياحية والمطاعم في المنطقة.", RateValue = 5 },
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-6), User = defaultUser2, UserId = defaultUser2.Id, Note = "وجبات لذيذة وتنوع رائع في قائمة الطعام.", RateValue = 4 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-SHARQ-06",
                        EntityName = "مجمع الشرق السياحي بحضاتهم",
                        ExpirationDate = DateTime.UtcNow.AddYears(5),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>
                    {
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAGcYnuKJoSeN-Cem6MAxm5O6GL932zKhhqb89XwEtp_F-95H9_9T1osYknl7FXPwONK28cBi6BzC8E8Hlg9-ynowSsgJxpYIDo634pg3qZ-9o-4wJoUDEzXiFFeCkcx8Gx3Ex4=w1920-h1080-k-no" },
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAEi7Hvi_en-naS2tiM4A1vbqnvbsacnf5oniU7ikRME6aAA6Lpxlec06n4uoP1ztGLNvu-40Q-QJlk3SUdhgFBNAM8MJT0W3Jpkq5fm_23qHxqaF6UNE4cgxdbT5SI63A6vrjWv=w1920-h1080-k-no" }
                    },
                    Phones = new List<PhoneNumber>()
                },

                // 7. بوفية مكه
                new Place
                {
                    PlaceName = "بوفية مكه",
                    Description = "بوفية وكافيه مميز يقدم المشروبات الساخنة والباردة والوجبات السريعة الخفيفة.",
                    Email = "makkah.buffet@example.com",
                    OpeningTime = new TimeOnly(6, 0),
                    ClosingTime = new TimeOnly(22, 0),
                    CreatedAt = DateTime.UtcNow.AddMonths(-3),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-10), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = cafeSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 15.0417625,
                        Longitude = 50.4740781,
                        DirectorateId = getGeoIds("2FRF+PJ4، الريدة الشرقية، اليمن").dirId,
                        DistrictId = getGeoIds("2FRF+PJ4، الريدة الشرقية، اليمن").distId,
                        NearestLandmark = "الريدة الشرقية الشارع العام",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-5), User = defaultUser1, UserId = defaultUser1.Id, Note = "شاي كرك رائع جداً وسريع التحضير.", RateValue = 5 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-MAKKAH-07",
                        EntityName = "بوفية مكه",
                        ExpirationDate = DateTime.UtcNow.AddYears(2),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>(),
                    Phones = new List<PhoneNumber>()
                },

                // 8. mood Cafe
                new Place
                {
                    PlaceName = "mood Cafe",
                    Description = "كافيه يتميز بأجواء عصرية وهادئة مناسب للشباب والمجموعات لتعديل المزاج.",
                    Email = "mood.cafe@example.com",
                    OpeningTime = new TimeOnly(16, 0),
                    ClosingTime = new TimeOnly(23, 0),
                    CreatedAt = DateTime.UtcNow.AddMonths(-1),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-5), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = cafeSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 15.0643041,
                        Longitude = 50.5780067,
                        DirectorateId = getGeoIds("3H7H+P6، رخميت، اليمن").dirId,
                        DistrictId = getGeoIds("3H7H+P6، رخميت، اليمن").distId,
                        NearestLandmark = "رخميت الشارع العام",
                    },
                    Rates = new List<Review>(),
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-MOOD-08",
                        EntityName = "mood Cafe",
                        ExpirationDate = DateTime.UtcNow.AddYears(3),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>(),
                    Phones = new List<PhoneNumber>()
                },

                // 9. قطرة كافية
                new Place
                {
                    PlaceName = "قطرة كافية",
                    Description = "كافيه مميز على كورنيش الشحر الجديد يقدم أفضل المشروبات الساخنة والباردة.",
                    Email = "qatarah.cafe@example.com",
                    OpeningTime = new TimeOnly(16, 30),
                    ClosingTime = new TimeOnly(21, 30),
                    CreatedAt = DateTime.UtcNow.AddMonths(-2),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-7), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = cafeSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 14.7586056,
                        Longitude = 49.6245923,
                        DirectorateId = getGeoIds("كورنيش الشحر الجديد، الشحر، اليمن").dirId,
                        DistrictId = getGeoIds("كورنيش الشحر الجديد، الشحر، اليمن").distId,
                        NearestLandmark = "كورنيش الشحر الجديد",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-1), User = defaultUser1, UserId = defaultUser1.Id, Note = "أجواء رائعة على البحر وقهوة ممتازة.", RateValue = 5 },
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-3), User = defaultUser2, UserId = defaultUser2.Id, Note = "الموقع خرافي والخدمة سريعة.", RateValue = 5 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-QATARAH-09",
                        EntityName = "قطرة كافية",
                        ExpirationDate = DateTime.UtcNow.AddYears(3),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>
                    {
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAFEDj_Q6VfqAQOAw2ZF2vMgVnB-LAfU0eBbrumjLHxCvAsINmcHvciWhT86s4inhXjk0GekpnIUupnYwMqlBgrTfNIIySapJT06s9NVV6SSS_6zvqEbgTwPbIRBNdQX7uWrP7GsniDMTWY=w1920-h1080-k-no" },
                        new PlaceImage { ImageUrl = "https://lh3.googleusercontent.com/gps-cs-s/APNQkAFwhYF3s23tOl7tZ1NfGn_MyMhibuRA4JbtQbAwOWKaJNVxYjOzu1BGBN9J39AQtsoZfq0G5myHF6Q4vr_4QAA043pVfmxODcZJ_H-4ougKQaZKZynU2efenBn1oIS-bwM1v1fhaDVeFyxt=w1920-h1080-k-no" }
                    },
                    Phones = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "780667945" }
                    }
                },

                // 10. مطبخ ومسلخ الحزالب
                new Place
                {
                    PlaceName = "مطبخ ومسلخ الحزالب",
                    Description = "مطعم ومسلخ يقدم أشهى اللحوم والوجبات الشعبية الحضرية.",
                    Email = "alhazaleb.kitchen@example.com",
                    OpeningTime = new TimeOnly(6, 0),
                    ClosingTime = new TimeOnly(22, 0),
                    CreatedAt = DateTime.UtcNow.AddMonths(-5),
                    UnavailableAt = [WeekDays.الجمعة],
                    Request = new Request { CreatedAt = DateTime.UtcNow.AddDays(-15), State = RequestStates.Accepted, UserId = defaultOwner.Id },
                    SubCategoryId = restaurantSub,
                    UserId = defaultOwner.Id,
                    Location = new Location
                    {
                        Latitude = 15.552727,
                        Longitude = 48.516388,
                        DirectorateId = getGeoIds("الحزالب، اليمن").dirId,
                        DistrictId = getGeoIds("الحزالب، اليمن").distId,
                        NearestLandmark = "الحزالب العام",
                    },
                    Rates = new List<Review>
                    {
                        new Review { CreatedAt = DateTime.UtcNow.AddDays(-10), User = defaultUser1, UserId = defaultUser1.Id, Note = "لحم طازج ومطهي بشكل ممتاز ولذيذ.", RateValue = 5 }
                    },
                    CommericalRegisteration = new CommericalRegisteration
                    {
                        CrNumber = "CR-HAZALEB-10",
                        EntityName = "مطبخ ومسلخ الحزالب",
                        ExpirationDate = DateTime.UtcNow.AddYears(2),
                        ImagePath = "",
                        User = defaultOwner,
                        UserId = defaultOwner.Id
                    },
                    Images = new List<PlaceImage>(),
                    Phones = new List<PhoneNumber>()
                }
            };

            context.Places.AddRange(placesToAdd);
            await context.SaveChangesAsync();
        }
    }
}