<html lang="en">
<title>Promo ID Optima</title>
<table>
    <tr>
        <td style="background-color: #ffff00; font-weight: bold;">Period</td>
        <td style="background-color: #ffff00; font-weight: bold;">Promo ID</td>
        <td style="background-color: #ffff00; font-weight: bold;">Entity</td>
        <td style="background-color: #ffff00; font-weight: bold;">Distributor</td>
        <td style="background-color: #ffff00; font-weight: bold;">Activity</td>
        <td style="background-color: #ffff00; font-weight: bold;">Activity Name</td>
        <td style="background-color: #ffff00; font-weight: bold;">Channel</td>
        <td style="background-color: #ffff00; font-weight: bold;">Account</td>
        <td style="background-color: #ffff00; font-weight: bold;">Promo Start</td>
        <td style="background-color: #ffff00; font-weight: bold;">Promo End</td>
        <td style="background-color: #ffff00; font-weight: bold;">Cost</td>
        <td style="background-color: #ffff00; font-weight: bold;">Cost >5bio</td>
        <td style="background-color: #ffff00; font-weight: bold;">Brand</td>
        <td style="background-color: #ffff00; font-weight: bold;">Trading Term / Adhoc</td>
        <td style="background-color: #ffff00; font-weight: bold;">Month Start</td>
        <td style="background-color: #ffff00; font-weight: bold;">Month End</td>
    </tr>
    @foreach($data->budgetPromoList as $r)
    <tr>
        <td>{{ $r->period }}</td>
        <td>{{ $r->promoId }}</td>
        <td>{{ $r->entity }}</td>
        <td>{{ $r->distributor }}</td>
        <td>{{ $r->activity }}</td>
        <td>{{ $r->activityName }}</td>
        <td>{{ $r->channel }}</td>
        <td>{{ $r->account }}</td>
        <td>{{ date('d-m-Y', strtotime($r->promoStart)) }}</td>
        <td>{{ date('d-m-Y', strtotime($r->promoEnd)) }}</td>
        <td>{{ number_format($r->investment) }}</td>
        <td>{{ $r->investment5bio }}</td>
        <td>{{ $r->brand }}</td>
        <td>{{ $r->tradingTermAdhoc }}</td>
        <td>{{ $r->monthStart }}</td>
        <td>{{ $r->monthEnd }}</td>
    </tr>
    @endforeach
</table>
</html>
