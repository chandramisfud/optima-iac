<table>
    <tbody>
    <tr>
        <td colspan="11" style="text-align: left; vertical-align: middle; font-weight:bold;">Master Mechanism</td>
    </tr>
    <tr>
        <td style="text-align: left; vertical-align: middle;">Date Retrieved : {{ date('Y-m-d') }}</td>
    </tr>
    <tr>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Entity</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Category</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Activity</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Sub Activity</th>
        <th style=" width: 60px; text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">SKU</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Channel</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Mechanism</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Start Date</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">End Date</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Created By</th>
        <th style="text-align: left; vertical-align: middle; font-weight:bold;  background-color: #0CA4FF;">Created On</th>

    </tr>
    @for ($i = 0; $i < count($data); $i++)
        <tr>
            <td>&#8203;{{ @$data[$i]->entity }}</td>
            <td>&#8203;{{ @$data[$i]->subCategory }}</td>
            <td>&#8203;{{ @$data[$i]->activity }}</td>
            <td>&#8203;{{ @$data[$i]->subActivity }}</td>
            <td>&#8203;{{ @$data[$i]->product }}</td>
            <td>&#8203;{{ @$data[$i]->channel }}</td>
            <td>&#8203;{{ @$data[$i]->mechanism  }}</td>
            <td>&#8203;{{ date('d-m-Y',strtotime(date(@$data[$i]->startDate))) }}</td>
            <td>&#8203;{{ date('d-m-Y',strtotime(date(@$data[$i]->endDate))) }}</td>
            <td>&#8203;{{ @$data[$i]->createBy  }}</td>
            <td>&#8203;{{ date('d-m-Y',strtotime(date(@$data[$i]->createdOn))) }}</td>
        </tr>
    @endfor
    </tbody>
</table>

