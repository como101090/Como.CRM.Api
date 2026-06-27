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
            string password)
        {
            var builder = new BodyBuilder();
            var image = builder.LinkedResources.Add("wwwroot/uploads/logos/como-logo.png");

           //  url = "https://bestbid.crm.comocode.am";
            return $"""
<!DOCTYPE html>
<html lang="hy">
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
    Հարգելի {companyName},
</h2>

<p style="margin:0;font-size:18px;color:#333;">
    Բարի գալուստ <strong>Como CRM</strong>։
</p>


            <p style="font-size:16px;color:#2d3436;line-height:28px;margin:0 0 20px 0;">
                Շնորհակալություն, որ ընտրել եք <strong>Como CRM</strong> համակարգը։
                Ձեր կազմակերպության հաշիվը հաջողությամբ ստեղծվել է։
            </p>

            <p style="font-size:16px;color:#2d3436;line-height:28px;margin:0 0 26px 0;">
                Համակարգ մուտք գործելու համար օգտագործեք ստորև ներկայացված տվյալները։
            </p>

            <table width="100%" cellpadding="0" cellspacing="0"
                   style="background:#f7fbfa;border:1px solid #cfe5df;border-radius:10px;padding:18px;">

                <tr>
                    <td style="padding:10px 0;width:190px;font-size:15px;color:#1f2d2b;font-weight:700;">
                        Համակարգի հասցե
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
                        Օգտանուն
                    </td>
                    <td style="padding:10px 0;font-size:15px;color:#2d3436;">
                        {userName}
                    </td>
                </tr>

                <tr>
                    <td style="padding:10px 0;font-size:15px;color:#1f2d2b;font-weight:700;">
                        Գաղտնաբառ
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
                    Մուտք գործել համակարգ
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

                <strong style="color:#004D49;">Խորհուրդ</strong><br/>

                Անվտանգության նկատառումներից ելնելով՝ առաջին մուտքից հետո
                խորհուրդ ենք տալիս փոխել Ձեր գաղտնաբառը։

            </div>

            <p style="font-size:15px;color:#4b5b58;line-height:26px;margin:28px 0 0 0;">
                Եթե մուտք գործելու ընթացքում ունենաք հարցեր կամ տեխնիկական խնդիրներ,
                մեր մասնագետները սիրով կօգնեն Ձեզ։
            </p>

            <p style="font-size:15px;color:#2d3436;line-height:26px;margin:28px 0 0 0;">
                Հարգանքով,<br/>
                <strong style="color:#004D49;">Como CRM թիմ</strong>
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

