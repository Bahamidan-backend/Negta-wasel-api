# واجهة برمجة تطبيقات دليل الأماكن والخدمات (Local-Places-Ranking-and-Evaluation-API)

مرحباً بك في مستودع الكود البرمجي الخاص بـ **Local-Places-Ranking-and-Evaluation-API**، وهي واجهة برمجة تطبيقات (Backend API) متكاملة مبنية باستخدام تقنية **.NET Core Web API** لتقديم خدمات دليل الأماكن والمتاجر والمرافق وتصنيفها وتقييمها.

يتميز النظام بهيكلية واضحة تفصل بين صلاحيات المستخدمين (الزبائن، وأصحاب الأعمال، والمسؤولين للنظام) مع دعم التقييمات، والتفاعلات، وإدارة التصنيفات الرئيسية والفرعية، والإشعارات.

---

## 🌟 مميزات النظام (Features)

*   **إدارة الهوية والوصول (Identity & Access Control):** نظام متكامل للمصادقة والتفويض يعتمد على `ASP.NET Core Identity` ورموز الأمان من نوع `JWT` (JSON Web Tokens). يدعم التسجيل، تأكيد البريد الإلكتروني برمز PIN، نسيت كلمة المرور، وتحديث الرموز (Refresh Tokens).
*   **لوحة تحكم الزبائن (Customer Portal):**
    *   البحث المتقدم عن الأماكن والمحلات وتصفيتها بحسب التصنيف، المنطقة (المديرية والحي)، سنة الافتتاح، الحد الأدنى للتقييم، وغيرها.
    *   إدارة قائمة الأماكن المفضلة.
    *   إدارة الحساب الشخصي (تغيير كلمة المرور، تعديل الملف الشخصي، تجميد أو إعادة تنشيط الحساب).
*   **لوحة تحكم أصحاب الأعمال (Owner Portal):**
    *   إرسال طلبات إضافة الأماكن والمحلات ومتابعتها.
    *   إدارة تفاصيل المحلات التابعة لهم ومتابعة أحدث المراجعات والتقييمات للعملاء.
    *   إحصائيات تفصيلية لأداء المحلات والزيارات.
*   **لوحة تحكم المسؤول (Admin Dashboard):**
    *   إدارة المستخدمين بالكامل (تفعيل، إيقاف، تعديل، حذف، بحث).
    *   إدارة طلبات إضافة الأماكن من أصحاب الأعمال (قبول أو رفض الطلبات).
    *   إدارة تصنيفات الأماكن الرئيسية والفرعية.
    *   إدارة المتاجر والمحلات وإيقافها مؤقتاً أو تفعيلها.
*   **نظام المراجعات والتقييمات (Reviews & Reactions):**
    *   كتابة تقييم ومراجعة للأماكن وتعديلها أو حذفها.
    *   التفاعل مع مراجعات المستخدمين الآخرين بالإعجاب أو التفاعل المناسب.
*   **نظام التنبيهات (Notification System):** تنبيه المستخدمين بالإجراءات والطلبات وتأكيد العمليات.

---

## 🛠️ التقنيات المستخدمة (Tech Stack)

*   **لغة البرمجة:** C# (نسخة .NET 8 أو أحدث)
*   **إطار العمل الأساسي:** ASP.NET Core Web API
*   **قاعدة البيانات:** PostgreSQL (باستخدام مكتبة `Npgsql.EntityFrameworkCore.PostgreSQL`)
*   **مُعرّف الهوية:** ASP.NET Core Identity لتوثيق المستخدمين
*   **الحماية والمصادقة:** JWT Bearer Authentication
*   **التوثيق التفاعلي:** Scalar API Reference (بديل حديث لـ Swagger UI مدمج لعرض وتجربة خدمات API)
*   **الحاويات:** Docker & Docker Compose لتسهيل بيئة التطوير والتشغيل

---

## 📁 هيكلية المجلدات والمشروع (Project Structure)

يتبع المشروع معمارية الطبقات المنفصلة لضمان سهولة الصيانة والتوسع:

*   **`WebAPI`**: طبقة العرض والتحكم (Presentation Layer). تحتوي على المتحكمات (Controllers)، وإعدادات التهيئة للخدمات والمكونات البرمجية (`Program.cs`, `ApiDependencyInjection.cs`) وملفات الإعدادات.
*   **`Application`** (أو `Application_Layer`): طبقة الأعمال والخدمات المشتركة. تحتوي على واجهات ومنطق الأعمال (Services)، ونماذج نقل البيانات (DTOs)، ومساعد المسارات والتوجيه (`Routing.cs`).
*   **`Core`** (أو `Domain_Layer`): طبقة النطاق الأساسية. تحتوي على كائنات النطاق (Entities)، والاستثناءات المخصصة، والقواعد الأساسية للأعمال.
*   **`infrastructural`** (أو `Persistence_Layer`): طبقة البنية التحتية وقاعدة البيانات. تحتوي على سياق قاعدة البيانات (`ApplicationDbContext`)، والتهجيرات وقاعدة البيانات (Migrations)، وإعدادات تخزين البيانات والوصول إليها.

---

## ⚙️ ملف الإعدادات وتهيئة البيئة (Environment Configuration)

يحتوي الملف `WebAPI/appsettings.json` على الإعدادات الأساسية لتشغيل التطبيق:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JwtConfig": {
    "ValidAudiences": "http://localhost:5000",
    "ValidIssuer": "http://localhost:5000",
    "Secret": "yourSecretKeyHereMustBeLongEnoughForHmacSha256",
    "ExpireMinutes": 525949
  },
  "Database": {
    "ConnectionString": "Host=localhost;Port=5432;Database=BackendAPI;Username=postgres;Password=postgres;Include Error Detail=true;"
  },
  "SmtpSettings": {
    "Server": "smtp.gmail.com",
    "Port": 465,
    "SenderName": "Mesh4All Support",
    "SenderEmail": "example@gmail.com",
    "Username": "example@gmail.com",
    "Password": "app-specific-password"
  }
}
```

> [!IMPORTANT]
> تأكد من تعديل سلسلة الاتصال بالـ Database وتعبئة إعدادات SMTP بشكل صحيح حتى يتمكن التطبيق من إرسال رسائل التحقق وتفعيل الحسابات بنجاح.

---

## 🚀 كيفية تشغيل المشروع (Getting Started)

### المتطلبات الأساسية
*   تنصيب حزمة التطوير **.NET 8 SDK** أو أحدث.
*   تنصيب خادم قاعدة البيانات **PostgreSQL** محلياً أو تشغيله عبر الحاويات.
*   (اختياري) تنصيب **Docker Desktop** للتشغيل الفوري.

### أولاً: التشغيل المحلي (Locally)

1.  **استنساخ المستودع:**
    ```bash
    git clone <repository-url>
    cd backend
    ```

2.  **إعداد قاعدة البيانات:**
    تأكد من تعديل `ConnectionString` في ملف `WebAPI/appsettings.json` ليتطابق مع بيانات خادم PostgreSQL الخاص بك.

3.  **تطبيق التهجيرات (Migrations):**
    قم بتطبيق جداول قاعدة البيانات عبر سطر الأوامر من المجلد الرئيسي للمشروع:
    ```bash
    dotnet ef database update --project infrastructural --startup-project WebAPI
    ```
    *(ملاحظة: يقوم التطبيق تلقائياً بمحاولة فحص وتطبيق الهجرات عند بدء التشغيل بفضل سكربت الترحيل التلقائي `AutomatedMigration.MigrateAsync`).*

4.  **تشغيل التطبيق:**
    ```bash
    dotnet run --project WebAPI
    ```
    سيبدأ التطبيق في العمل وغالباً ما يستمع على المنفذ المحرز في بيئة التطوير (مثلاً `http://localhost:5000` أو `https://localhost:5001`).

---

### ثانياً: التشغيل باستخدام Docker Compose

يمكنك تشغيل خادم قاعدة البيانات وتطبيق الويب بلمسة واحدة باستخدام ملف `compose.yaml` المرفق في جذر المشروع:

```bash
docker compose up -d --build
```

سيعمل هذا الأمر على:
1. تشغيل قاعدة بيانات PostgreSQL على المنفذ الخارجي `5433` (والداخلي `5432`).
2. بناء تطبيق الويب وتشغيله على المنفذ `5000` (الخارجي) ليوجه للداخل على `8080`.
3. ربط الحاويات معاً وتطبيق الهجرات تلقائياً.

---

## 📄 توثيق خدمات الـ API (Scalar API Reference)

يستخدم هذا المشروع واجهة **Scalar** التفاعلية والحديثة بدلاً من Swagger التقليدي. 
يمكنك استعراض جميع خدمات الـ API ونقاط النهاية، واختبار الإرسال والاستقبال مباشرةً بعد تشغيل التطبيق بالتوجه للرابط التالي في المتصفح:

*   **رابط التوثيق التفاعلي:** `http://localhost:5000/docs`
*   **ملف مواصفات OpenAPI:** `http://localhost:5000/openapi/v1.json`

---

## 🛣️ مسارات ونقاط النهاية للـ API (Endpoints Reference)

فيما يلي قائمة شاملة بمسارات التطبيق مقسمة حسب الصلاحيات والوظائف الأساسية (حيث تبدأ كافة المسارات بالسابقة `/api/`):

### 1. المصادقة وإدارة الحسابات (Authentication)
تخدم هذه الخدمات عمليات التحقق والأمان والوصول إلى الحساب:

| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/auth/register` | `POST` | إنشاء حساب جديد للمستخدم (زبون أو صاحب عمل). |
| `/api/auth/login` | `POST` | تسجيل الدخول واستلام رموز الوصول `AccessToken` و `RefreshToken`. |
| `/api/auth/refresh-token` | `POST` | تجديد رمز الوصول المنتهي باستخدام رمز التحديث. |
| `/api/auth/forget-password` | `POST` | طلب إرسال رمز استعادة كلمة المرور عبر البريد. |
| `/api/auth/reset-password` | `POST` | إعادة تعيين كلمة المرور الجديدة باستخدام الرمز المستلم. |
| `/api/auth/is-email-verified` | `POST` | التحقق مما إذا كان البريد الإلكتروني مؤكداً أم لا. |
| `/api/auth/email-confirm` | `GET` | تأكيد حساب البريد الإلكتروني عبر رمز التفعيل (PIN). |
| `/api/auth/ChangeEmailConfirm` | `GET` | تأكيد تغيير البريد الإلكتروني للمستخدم. |
| `/api/auth/resend-email-confirmation` | `GET` | إعادة إرسال رسالة تفعيل البريد الإلكتروني. |
| `/api/auth/logout` | `GET` | تسجيل الخروج وإلغاء صلاحية رمز الوصول الحالية. |

---

### 2. خدمات الأماكن والمحلات للعملاء (Customer Places)
لتمكين الزوار من استعراض وبحث الأماكن والمتاجر المتاحة على النظام:

| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/customer/places/search/{placeName}` | `GET` | البحث المتقدم عن الأماكن بالاسم والمنطقة والتصنيف وغيرها. |
| `/api/customer/places/{placeId}` | `GET` | جلب تفاصيل مكان محدد باستخدام معرّف المكان الرقمي. |
| `/api/customer/places/getall` | `GET` | استرجاع كافة الأماكن النشطة في النظام (مع دعم الصفحات). |
| `/api/customer/places/Filter` | `GET` | تصفية الأماكن وترتيبها حسب التقييم أو الاسم أو الأحدث. |
| `/api/customer/places/category` | `POST` | طلب الأماكن التابعة لتصنيف معين عبر نموذج الطلب. |
| `/api/customer/places/category/{categoryId}` | `GET` | جلب الأماكن التابعة لتصنيف رئيسي بالمعرّف. |
| `/api/customer/places/subcategory/{subCategoryId}` | `GET` | جلب الأماكن التابعة لتصنيف فرعي بالمعرّف. |
| `/api/customer/places/{placeId}/Detail` | `GET` | تفاصيل التقييمات والأرقام والإحصائيات الخاصة بالمكان. |
| `/api/customer/places/{placeId}/reactions` | `GET` | جلب التقييمات والتفاعلات الخاصة بمكان محدد. |

---

### 3. نظام المراجعات والتقييمات والتفاعل (Reviews & Reactions)
تسمح للمستخدمين بمشاركة آرائهم والتفاعل مع آراء الآخرين:

| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/reviews/{placeId}` | `GET` | عرض كافة التقييمات والمراجعات المكتوبة لمكان معين. |
| `/api/reviews/myReviews` | `GET` | استرجاع كافة المراجعات التي كتبها المستخدم الحالي. |
| `/api/reviews/Review/{reviewId}` | `GET` | جلب مراجعة وتقييم محدد بمعرّف المراجعة. |
| `/api/reviews/{placeId}` | `POST` | كتابة ونشر تقييم ومراجعة جديدة لمكان محدد. |
| `/api/reviews/{reviewId}` | `PUT` / `PATCH` | تعديل وتحديث مراجعة حالية. |
| `/api/reviews/{reviewId}` | `DELETE` | حذف مراجعة معينة كتبها المستخدم. |
| `/api/reviews/check/{placeId}` | `GET` | التحقق مما إذا كان للمستخدم الحالي تقييم مسبق لهذا المكان. |
| `/api/reviews/{reviewId}/reaction` | `POST` | إضافة تفاعل (Like/Dislike) على مراجعة معينة. |
| `/api/reviews/{reviewId}/reaction-type` | `GET` | التحقق من نوع تفاعل المستخدم الحالي على المراجعة. |

---

### 4. المفضلة وإعدادات حساب العميل (Favourites & Settings)

| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/customer/favourites` | `GET` | جلب قائمة الأماكن التي يفضلها العميل. |
| `/api/customer/favourites/{placeId}` | `POST` | إضافة مكان محدد إلى قائمة المفضلة. |
| `/api/customer/favourites/{placeId}` | `DELETE` | إزالة مكان محدد من قائمة المفضلة. |
| `/api/customer/favourites/{placeId}` | `GET` | التحقق مما إذا كان المكان موجوداً في قائمة المفضلة للعميل. |
| `/api/customer/changePassword` | `POST` | تغيير كلمة المرور للمستخدم الحالي. |
| `/api/customer/GetProfileInfo` | `GET` | جلب معلومات الملف الشخصي الأساسية للمستخدم. |
| `/api/customer/ChangeProfile` | `PUT` / `POST` | تحديث بيانات الملف الشخصي. |
| `/api/customer/deactivate` | `POST` | تعطيل وتجميد حساب العميل مؤقتاً. |
| `/api/customer/reactivate` | `POST` | إعادة تنشيط الحساب المعطل مجدداً. |

---

### 5. إدارة المالك لأماكنه (Owner Places & Dashboard)
مخصصة لأصحاب المحلات والأعمال لإدارة أماكنهم المعروضة في الدليل:

| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/owner/places` | `GET` | جلب قائمة جميع الأماكن التابعة للمالك الحالي. |
| `/api/owner/places/{id}/details` | `GET` | جلب تفاصيل وإحصائيات متكاملة لمكان معين تابع للمالك. |
| `/api/owner/places/{id}` | `GET` | جلب بيانات المكان للتعديل أو العرض. |
| `/api/owner/places` | `POST` | تقديم طلب إضافة مكان/محل جديد في النظام. |
| `/api/owner/places/{id}` | `PUT` | تحديث بيانات مكان معلق أو نشط تابع للمالك. |
| `/api/owner/places/{id}` | `DELETE` | حذف أو إلغاء متجر للمالك. |
| `/api/owner/places/directorates` | `GET` | جلب قائمة المديريات المتاحة لإعداد موقع المتجر. |
| `/api/owner/places/LatestReviews/{id}` | `GET` | جلب آخر المراجعات والتقييمات التي كتبها العملاء على هذا المكان. |
| `/api/owner/dashboard/places` | `GET` | جلب الأماكن التابعة للمالك لعرضها في لوحة معلومات المالك. |
| `/api/owner/dashboard/statistics` | `GET` | عرض إحصائيات عامة لأداء المحلات والتقييمات التابعة للمالك. |

---

### 6. إدارة المسؤول للنظام (Admin Controls)
خدمات إدارية ذات صلاحيات عالية متاحة فقط للمسؤولين (Administrators):

#### إدارة طلبات إضافة الأماكن (Requests Management)
| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/admin/requests` | `GET` | عرض كافة طلبات إضافة الأماكن المقدمة من الملاك ومتابعة حالتها. |
| `/api/admin/requests/{id}/details` | `GET` | جلب التفاصيل الكاملة للموقع أو المتجر المطلوب إضافته. |
| `/api/admin/requests/{orderId}/accept` | `POST` | قبول الطلب ونشر المتجر رسمياً في دليل الأماكن. |
| `/api/admin/requests/reject` | `POST` | رفض الطلب مع توضيح أسباب الرفض للمالك. |

#### إدارة المتاجر القائمة (Stores Management)
| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/admin/stores` | `GET` | استعراض كافة المحلات والمتاجر الموجودة بالنظام. |
| `/api/admin/stores/statistics` | `GET` | الحصول على إحصائيات عامة للنظام (إجمالي المتاجر، النشطة، إلخ). |
| `/api/admin/stores/{id}/suspend` | `POST` | إيقاف متجر مؤقتاً وحجبه عن العملاء لأسباب إدارية. |
| `/api/admin/stores/{id}/activate` | `POST` | إعادة تنشيط المتجر الموقوف مجدداً. |
| `/api/admin/stores/{id}` | `DELETE` | حذف المتجر بشكل نهائي من النظام. |
| `/api/admin/stores/{id}/status` | `POST` | تغيير حالة المتجر مباشرةً. |
| `/api/admin/stores/edit` | `POST` | تعديل وتحديث بيانات متجر إدارياً. |
| `/api/admin/stores/{id}/edit` | `GET` | جلب بيانات المتجر لتحريرها من قبل المدير. |

#### إدارة الأعضاء والمستخدمين (Users Management)
| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/admin/users` | `GET` | جلب قائمة كافة مستخدمي النظام بمختلف أدوارهم. |
| `/api/admin/users/{id}/suspend` | `POST` | حظر وإيقاف حساب مستخدم. |
| `/api/admin/users/{id}/activate` | `POST` | إلغاء حظر حساب مستخدم وتفعيله مجدداً. |
| `/api/admin/users/{id}/status` | `POST` | تغيير حالة حساب المستخدم يدوياً. |
| `/api/admin/users` | `POST` | إنشاء حساب مستخدم جديد بصلاحيات محددة مباشرةً من المدير. |
| `/api/admin/users` | `DELETE` | حذف مستخدم نهائياً من النظام. |
| `/api/admin/users` | `PUT` | تحديث بيانات مستخدم معين. |
| `/api/admin/users/find` | `GET` | البحث المتقدم عن مستخدمين بالاسم أو البريد. |

#### لوحة تحكم وإحصائيات الإدارة العامة (Admin Dashboard)
| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/admin/dashboard/latest-Request` | `GET` | عرض قائمة بأحدث طلبات الإضافة المستلمة للتسهيل على المدير. |
| `/api/admin/dashboard/Request-status` | `GET` | إحصائيات سريعة عن نسب قبول ورفض الطلبات في النظام. |
| `/api/admin/dashboard/{id}` | `GET` | جلب بيانات طلب محدد بالمعرّف لعرضه السريع. |

---

### 7. إدارة التصنيفات الرئيسية والفرعية (Categories & Subcategories)
العمليات الإدارية والعامة على تنظيم وتصنيف الأماكن:

| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/admin/categories` | `GET` | جلب كافة التصنيفات الرئيسية في النظام. |
| `/api/admin/categories/getCategory` | `GET` | البحث عن تصنيف رئيسي بالاسم. |
| `/api/admin/categories/{id}` | `GET` | جلب تصنيف رئيسي بالمعرّف. |
| `/api/admin/categories` | `POST` | إنشاء تصنيف رئيسي جديد. |
| `/api/admin/categories` | `PUT` | تحديث وتعديل بيانات تصنيف رئيسي. |
| `/api/admin/categories/{id}` | `DELETE` | حذف تصنيف رئيسي (حذف معرّف). |
| `/api/subcategories/with-categories` | `GET` | جلب كافة التصنيفات الفرعية مدمجة مع تفاصيل تصنيفاتها الرئيسية. |
| `/api/subcategories/category/{categoryId}` | `GET` | استرجاع التصنيفات الفرعية التابعة لتصنيف رئيسي محدد. |
| `/api/subcategories` | `POST` | إضافة وتصنيف فرعي جديد. |
| `/api/subcategories/{subCategoryId}` | `PUT` | تحديث بيانات تصنيف فرعي محدد بالمعرّف. |
| `/api/subcategories/{subCategoryId}` | `DELETE` | حذف تصنيف فرعي نهائياً من النظام. |

---

### 8. الإشعارات (Notifications)
للمتابعة الحية للنشاطات والأحداث داخل النظام:

| المسار (Route) | نوع الطلب (HTTP Method) | الوصف (Description) |
| :--- | :---: | :--- |
| `/api/notifications/AllNotifications` | `GET` | جلب جميع التنبيهات الخاصة بالمستخدم الحالي. |

---

## 🔒 استجابات الخطأ وتوحيد العائد (API Result Conventions)

تم إعداد النظام لإرجاع استجابات موحدة ومترجمة للغة العربية لتسهيل الفهم البرمجي والتشغيل:
*   **خطأ 404 (NotFound):** يرجع رسالة `"لم يتم إيجاد المصدر"` في الحالات التي لا يتوفر فيها الكيان.
*   **خطأ 400 (BadRequest):** يرجع رسالة `"البيانات غير صالحة"` مرفقاً بها قائمة تفصيلية بالأخطاء في حقول المدخلات.
*   **خطأ 401 (Unauthorized):** يرجع رسالة `"الرجاء تسجيل الدخول"`.
*   **خطأ 403 (Forbidden):** يرجع رسالة `"غير مصرح لك بالوصول إلى المصدر"`.
*   **خطأ 409 (Conflict):** يرجع رسالة `"تعارض في البيانات"` (مثل محاولة تسجيل مستخدم ببريد مكرر).
*   **خطأ 500 (InternalServerError):** يرجع رسالة `"حدث خطأ داخلي"`.
# Negta-wasel-api
