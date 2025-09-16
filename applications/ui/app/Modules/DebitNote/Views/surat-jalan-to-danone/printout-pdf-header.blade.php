<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title></title>

    <style>
        body {
            font-family: Sans-serif;
        }

        .title_doc {
            font-size: 26pt;
            margin-bottom: 20px;
        }

        .info_tt {
            width: 40%;
            /* border: 1px solid #000; */
            margin-bottom: 30px;
        }

        .space_header {
            width: 20px;
            padding: 5px;
            margin: 20px;
        }
    </style>
</head>
<body>
<div class="title_doc">
    Tanda Terima DN
</div>

<table class="info_tt">
    <tr>
        <td width="40%">Entity</td>
        <td>{{ @$data->entityDesc }}</td>
    </tr>
    <tr>
        <td>Distributor</td>
        <td>{{ @$data->distributorDesc }}</td>
    </tr>
    <tr>
        <td>No Tanda Terima</td>
        <td>{{ @$data->refId }}</td>
    </tr>
    <tr>
        <td>Tanggal</td>
        <td>{{ @$data->createOn }}</td>
    </tr>
</table>

<div class="space_header">
</div>
</body>
</html>
