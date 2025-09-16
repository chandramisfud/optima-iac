<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title></title>
    <style>
     @media screen{
        @font-face {
    font-family: 'Calibri';
        }
    }
        body { 
            /* font-family: "Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif"; */
            font-family: "Calibri";
            font-size: 12px;
            font-weight: 300;
            font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif;
        }
        /* table tr td { padding: 5px 10px 5px 10px; vertical-align: top; } */
        td{
            font-family: Calibri, Arial, Helvetica, sans-serif;
        }
        .header_id { width: 100%; margin-bottom: 10px; }
        .header_id tr td { font-size: 12pt; }
        .header_id table tr > .cell_label { width:250px; border:1px solid black}
        .header_id table tr > .cell_separator { width: 10px; }
        .header_title { margin-bottom: 10px; }
        .header_title table { width: 100%; background-color: lightgrey; border: 1px solid black; border-collapse: collapse; text-align: center; }
        .header_title table tr td { font-size: 20px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; }
        .date_section > .table_left { width: 60%; float: left; }
        .date_section > .table_left tr > .cell_label { width: 250px; }
        .date_section > .table_left tr > .cell_separator { width: 10px; }
        .date_section > .table_right { width: 40%; float: right; }
        .date_section > .table_right tr > .cell_label { width: 100px; border: 1px solid lightgrey}
        .date_section > .table_right tr > .cell_separator { width: 10px; }
        .detail_content { margin-bottom: 10px; }
        .detail_content table { width: 100%; }
        .detail_content table  tr > .cell_label { width: 250px; }
        .detail_content table  tr > .cell_separator { width: 10px; }

        .detail_mechanism > table { width: 100%; border: 1px solid black; border-collapse: collapse; float: left; margin-top: 20px; margin-bottom: 20px; }
        .detail_mechanism > table .detail_mechanism_title > .col_left { border-right: 1px solid black; width: 50%; font-size: 14px; font-weight: bold; text-decoration: underline; }
        .detail_mechanism > table .detail_mechanism_title > .col_right { border-left: 1px solid black; width: 50%; font-size: 14px; font-weight: bold; text-decoration: underline; }
        .detail_mechanism > table .detail_mechanism_content > .col_left { border-right: 1px solid black; width: 50%; height: 100px; vertical-align: top; padding-left: 20px; }
        .detail_mechanism > table .detail_mechanism_content > .col_right { border-left: 1px solid black; width: 50%; height: 100px; vertical-align: top; padding-left: 20px; }

        .detail_value table { width: 100%; border: 1px solid black; border-collapse: collapse; float: left; margin-bottom: 20px; }
        .detail_value table tr { border: 1px solid black; }
        .detail_value table tr td { border: 1px solid black; }

        .detail_analysis table { width: 100%; border: 1px solid black; border-collapse: collapse; float: left; margin-bottom: 20px; }
        .detail_analysis table .detail_analysis_title td { border: 1px solid black; text-align: center; }
        .detail_analysis table .detail_analysis_content td { border-right: 1px solid black; }

        .detail_approval table { width: 100%; border: 1px solid black; border-collapse: collapse; float: left; }
        .detail_approval table .detail_approval_title td { text-align: center; border: 1px solid black; }
        .detail_approval table .detail_approval_content td { border-right: 1px solid black; text-align: center; height: 150px; vertical-align: bottom; }
        .detail_approval table .detail_approval_content td .detail_approval_on { border: 0px solid black; padding:5px 5px 5px 5px; width: 100px; margin: auto; margin-bottom: 10px; }
        .blank_row
        {
            height: 10px !important; /* overwrites any other rules */
            /* background-color: #FFFFFF; *//
        }
    </style>
</head>
<body>

        <table>
           
            <tr>
            <td colspan="3" style="font-size: 16px;font-weight: 600">Blitz Transfered at {{ date("M d Y") }} 5:00 AM</td>
            </tr>
            <tr>
            <td colspan="3" style="border: 1px solid #000; text-align: center; font-weight: 600">BLITZ Transfer - Unmapped Attributes</td>
            </tr>
            <tr>
                <td style="padding:5px; border: 1px solid #000; text-align: center; font-weight: 600">Type</td>
                <td style="padding:5px; border: 1px solid #000; text-align: center; font-weight: 600">Code</td>
                <td style="padding:5px; border: 1px solid #000; text-align: center; font-weight: 600">Desc</td>
            </tr>
            @foreach(@$data as $dt)
            <tr>
                <td style="padding:5px; border: 1px solid #000;">{{ $dt ->type }}</td>
                <td style="padding:5px; border: 1px solid #000;">{{ $dt ->code }}</td>
                <td style="padding:5px; border: 1px solid #000;">{{ $dt ->desc }}</td>
            </tr>
          @endforeach
          <!-- <tr>
                <td style="border: 1px solid grey;">{{ @$data2 ->email_to }}</td>
                <td style="border: 1px solid grey;">{{ @$data2 ->email_cc }}</td>
                <td style="border: 1px solid grey;">{{ @$data2 ->email_subject }}</td>
            </tr> -->
        </table>

    

</body>
</html>
