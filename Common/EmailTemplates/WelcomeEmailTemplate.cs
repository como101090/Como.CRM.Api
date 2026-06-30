using Como.CRM.Api.Enums;
using MimeKit;
using static System.Net.WebRequestMethods;

namespace Como.CRM.Api.Common.EmailTemplates
{

    public static class WelcomeEmailTemplate
    {
        public static string Build(
     string companyName,
     string url,
     string userName,
     string password,
     Language language)
        {
            string htmlLang;
            string dear;
            string welcome;
            string accountCreated;
            string loginInfo;
            string systemUrl;
            string userNameText;
            string passwordText;
            string loginButton;
            string adviceTitle;
            string adviceText;
            string supportText;
            string regards;
            string team;

            switch (language)
            {
                case Language.En:
                    htmlLang = "en";
                    dear = "Dear";
                    welcome = "Welcome to";
                    accountCreated = "Thank you for choosing <strong>Como CRM</strong>. Your company account has been successfully created.";
                    loginInfo = "Please use the credentials below to access the system.";
                    systemUrl = "System URL";
                    userNameText = "Username";
                    passwordText = "Password";
                    loginButton = "Log In";
                    adviceTitle = "Recommendation";
                    adviceText = "For security reasons, we recommend changing your password after your first login.";
                    supportText = "If you have any questions or experience technical issues while logging in, our support team will be happy to assist you.";
                    regards = "Best regards,";
                    team = "Como CRM Team";
                    break;


                case Language.Ru:
                    htmlLang = "ru";
                    dear = "Уважаемый";
                    welcome = "Добро пожаловать";
                    accountCreated = "Спасибо, что выбрали <strong>Como CRM</strong>. Учетная запись вашей компании успешно создана.";
                    loginInfo = "Для входа в систему используйте следующие данные.";
                    systemUrl = "Адрес системы";
                    userNameText = "Имя пользователя";
                    passwordText = "Пароль";
                    loginButton = "Войти в систему";
                    adviceTitle = "Рекомендация";
                    adviceText = "В целях безопасности рекомендуем изменить пароль после первого входа.";
                    supportText = "Если у вас возникнут вопросы или технические проблемы, наша команда будет рада помочь.";
                    regards = "С уважением,";
                    team = "Команда Como CRM";
                    break;

                case Language.Ka:
                    htmlLang = "ka";
                    dear = "ძვირფასო";
                    welcome = "კეთილი იყოს თქვენი მობრძანება";
                    accountCreated = "გმადლობთ, რომ აირჩიეთ <strong>Como CRM</strong>. თქვენი კომპანიის ანგარიში წარმატებით შეიქმნა.";
                    loginInfo = "სისტემაში შესასვლელად გამოიყენეთ ქვემოთ მოცემული მონაცემები.";
                    systemUrl = "სისტემის მისამართი";
                    userNameText = "მომხმარებლის სახელი";
                    passwordText = "პაროლი";
                    loginButton = "სისტემაში შესვლა";
                    adviceTitle = "რეკომენდაცია";
                    adviceText = "უსაფრთხოების მიზნით, გირჩევთ პირველი შესვლის შემდეგ შეცვალოთ პაროლი.";
                    supportText = "თუ კითხვები ან ტექნიკური პრობლემები შეგექმნებათ, ჩვენი გუნდი სიამოვნებით დაგეხმარებათ.";
                    regards = "პატივისცემით,";
                    team = "Como CRM გუნდი";
                    break;

                case Language.Hy:
                default:
                    htmlLang = "hy";
                    dear = "Հարգելի";
                    welcome = "Բարի գալուստ";
                    accountCreated = "Շնորհակալություն, որ ընտրել եք <strong>Como CRM</strong> համակարգը։ Ձեր կազմակերպության հաշիվը հաջողությամբ ստեղծվել է։";
                    loginInfo = "Համակարգ մուտք գործելու համար օգտագործեք ստորև ներկայացված տվյալները։";
                    systemUrl = "Համակարգի հասցե";
                    userNameText = "Օգտանուն";
                    passwordText = "Գաղտնաբառ";
                    loginButton = "Մուտք գործել համակարգ";
                    adviceTitle = "Խորհուրդ";
                    adviceText = "Անվտանգության նկատառումներից ելնելով՝ առաջին մուտքից հետո խորհուրդ ենք տալիս փոխել Ձեր գաղտնաբառը։";
                    supportText = "Եթե մուտք գործելու ընթացքում ունենաք հարցեր կամ տեխնիկական խնդիրներ, մեր մասնագետները սիրով կօգնեն Ձեզ։";
                    regards = "Հարգանքով,";
                    team = "Como CRM թիմ";
                    break;
            }

            return $"""
<!DOCTYPE html>
<html lang="{htmlLang}">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>Como CRM</title>
</head>

<body style="margin:0;padding:30px;background:#f4f8f7;font-family:Arial,Helvetica,sans-serif;">

<table align="center" width="650" cellpadding="0" cellspacing="0"
       style="background:#ffffff;border-radius:12px;overflow:hidden;border:1px solid #d9ebe7;box-shadow:0 4px 18px rgba(0,70,65,0.08);">

    <tr>
        <td style="background:#009688;height:12px;"></td>
    </tr>

<tr>
    <td align="center" style="padding:34px 35px 24px 35px;background:#ffffff;">
        <img src="https://crm.comocode.am/uploads/logos/como-logo.png"
             alt="Como CRM"
             style="max-width:280px;height:auto;display:block;" />
    </td>
</tr>

    <tr>
        <td style="padding:0 35px;">
            <div style="height:2px;background:#cfe5df;"></div>
        </td>
    </tr>

    <tr>
        <td style="padding:35px 40px 40px 40px;">

           <h2 style="margin:0 0 22px 0;color:#004D49;font-size:26px;line-height:34px;font-weight:700;">
                {dear} {companyName}
           </h2>

            <p style="margin:0;font-size:18px;color:#333;">
                {welcome} <strong>Como CRM</strong>
            </p>

            <p style="font-size:16px;color:#2d3436;line-height:28px;margin:0 0 20px 0;">
                {accountCreated}
            </p>

            <p style="font-size:16px;color:#2d3436;line-height:28px;margin:0 0 26px 0;">
                {loginInfo}
            </p>

            <table width="100%" cellpadding="0" cellspacing="0"
                   style="background:#f7fbfa;border:1px solid #cfe5df;border-radius:10px;padding:18px;">

                <tr>
                    <td style="padding:10px 0;width:190px;font-size:15px;color:#1f2d2b;font-weight:700;">
                        {systemUrl}
                    </td>
                    <td style="padding:10px 0;font-size:15px;">
                        <a href="{url}" target="_blank"
                           style="color:#006C64;text-decoration:underline;font-weight:600;">
                            {url}
                        </a>
                    </td>
                </tr>

                <tr>
                    <td style="padding:10px 0;font-size:15px;color:#1f2d2b;font-weight:700;">
                        {userNameText}
                    </td>
                    <td style="padding:10px 0;font-size:15px;color:#2d3436;">
                        {userName}
                    </td>
                </tr>

                <tr>
                    <td style="padding:10px 0;font-size:15px;color:#1f2d2b;font-weight:700;">
                        {passwordText}
                    </td>
                    <td style="padding:10px 0;font-size:15px;color:#2d3436;">
                        {password}
                    </td>
                </tr>

            </table>

            <div style="text-align:center;margin:30px 0;">

                <a href="{url}" target="_blank"
                   style="
                        display:inline-block;
                        background:#009688;
                        color:#ffffff;
                        padding:16px 42px;
                        border-radius:8px;
                        text-decoration:none;
                        font-size:17px;
                        font-weight:700;
                        box-shadow:0 8px 18px rgba(0,77,73,0.22);">
                    {loginButton}
                </a>

            </div>

            <div style="
                    background:#f5fbf8;
                    border:1px solid #cfe5df;
                    border-left:5px solid #31A84A;
                    border-radius:8px;
                    padding:18px 20px;
                    color:#24413d;
                    line-height:26px;
                    font-size:15px;">

                <strong style="color:#004D49;">{adviceTitle}</strong><br/>

                {adviceText}

            </div>

            <p style="font-size:15px;color:#4b5b58;line-height:26px;margin:28px 0 0 0;">
                {supportText}
            </p>

            <p style="font-size:15px;color:#2d3436;line-height:26px;margin:28px 0 0 0;">
                {regards}<br/>
                <strong style="color:#004D49;">{team}</strong>
            </p>

        </td>
    </tr>

    <tr>
        <td align="center"
            style="background:#009688;color:#ffffff;padding:22px;font-size:13px;">
            © 2026 Como CRM • All Rights Reserved
        </td>
    </tr>

</table>

</body>
</html>
""";
        }
    }

}
