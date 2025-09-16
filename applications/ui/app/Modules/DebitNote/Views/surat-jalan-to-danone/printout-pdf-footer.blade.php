<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title></title>

    <style>
        .footer {
            text-align: center;
        }

        .text_footer {
            font-size: 8pt;
            width: 100%;
        }
    </style>
</head>
<body>
<div class="footer">
    <table class="text_footer">
        <tr>
            <td width="20%" align="left">Generate by :  {{ @$data->user_print }}</td>
            <td width="60%"></td>
            <td width="20%" align="right">Print date : {{ date("d/m/Y h:i:s") }}</td>
        </tr>
    </table>
</div>

</body>
</html>
