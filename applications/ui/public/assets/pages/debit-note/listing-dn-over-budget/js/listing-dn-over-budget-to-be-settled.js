'use strict';

let dt_listing_dn_over_budget_to_be_settled;
let swalTitle = "DN Over Budget To Be Settled";
heightContainer = 295;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });


    dt_listing_dn_over_budget_to_be_settled = $('#dt_listing_dn_over_budget_to_be_settled').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/listing-over-budget/list/to-be-settled',
            type: 'GET',
        },
        processing: true,
        serverSide: true,
        paging: true,
        ordering: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                width: 20,
                className: 'dt-control align-middle',
                orderable: false,
                data: null,
                defaultContent: '',
            },
            {
                targets: 1,
                title: 'Promo Number',
                data: 'refId',
                width: 150,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    let startYear = new Date(full['startPromo']).getFullYear();
                    if (startYear >= 2025) {
                        return '<a href="/promo/creation/form?method=update&promoId=' + full.id + '&c='+ full.categoryShortDescEnc +'&old=0&recon=1" class="text-optima text-hover-darkblue">' + data + '</a>';
                    } else {
                        return '<a href="/promo/recon/form?method=update&promoId=' + full.id + '&c='+ full.categoryShortDescEnc +'" class="text-optima text-hover-darkblue">' + data + '</a>';
                    }
                }
            },
            {
                targets: 2,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Promo Start',
                data: 'startPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDateOptima(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 5,
                title: 'Promo End',
                data: 'endPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDateOptima(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 6,
                title: 'DN Claim',
                data: 'dpp',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 7,
                title: 'Investment',
                data: 'investment',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0);
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
        footerCallback: function ( row, data, start, end, display ) {
            let api = this.api();
            let rows = api.rows( { selected: true } ).indexes();
            let intVal = function ( i ) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '')*1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // Total over all pages
            let total = api
                .column( 7, {filter:'applied'} )
                .data()
                .reduce( function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0 );

            // console.log('data total', data)
            $('.dt-footer').html("<div>Total Investment : <span id='tot'>" + formatMoney(total, 0) + "</span></div>" );
        },
    });

    $('#dt_listing_dn_over_budget_to_be_settled tbody').on('click', 'td.dt-control', function () {
        let tr = $(this).closest('tr');
        let row = dt_listing_dn_over_budget_to_be_settled.row( tr );
        if ( row.child.isShown() ) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            $.ajax({
                url         : "/dn/listing-over-budget/list/to-be-settled/promo-id",
                type        : "GET",
                dataType    : 'json',
                data        : {promoId: row.data().id},
                async       : true,
                beforeSend: function( jqXHR ) {
                    $('.dataTables_processing').css('display', 'block')
                },
                success: function(result)
                {
                    let data = result.data;
                    let i;
                    let text="";
                    for (i = 0; i < data.length; i++) {
                        let checked = ( (data[i].vatExpired) ? "checked" : "");
                        let vatExpired = '\
							<span class="form-check form-switch form-check-custom form-check-success form-check-solid">\
                                <label><input type="checkbox" class="form-check-input vat-expired" ' + checked + ' data-toggle="toggle" disabled>\
                                </label>\
                            </span>\
						';


                        text += '<tr>'+
                            '<td>'+data[i].refId+'</td><td>'+data[i].activityDesc+'</td><td class="text-end">'+ formatMoney(data[i].dpp, 0) +'</td><td class="text-end">'+ formatMoney(data[i].totalClaim, 0) +'</td><td class="text-end">'+ formatMoney(data[i].ppnAmt, 0) +'</td><td>' + vatExpired + '</td>' +
                            '</tr>';

                    }
                    let res = '<table class="table table-sm child-dt" style="margin-left:20px; width: 95%;">'+
                        '<thead><tr style="background-color: lightgray">'+
                        '<th class="fw-bolder">DN Number</th><th class="fw-bolder" width="40%">Activity</th><th class="text-end fw-bolder">DPP</th><th class="text-end fw-bolder">Total Claim</th><th class="text-end fw-bolder">VAT</th><th class="fw-bolder">VAT Expired</th>'+
                        '</tr></thead>'+ text +
                        '</table>';

                    row.child( res ).show();
                    tr.addClass('shown');
                },
                complete: function(response) {
                    $('.dataTables_processing').css('display', 'none')
                },
                error: function (jqXHR, textStatus, errorThrown)
                {
                    $('.dataTables_processing').css('display', 'none')
                    console.log(jqXHR.responseText);
                }
            });

        }
    });
});
