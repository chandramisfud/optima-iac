'use strict';

let swalTitle = "Promo Calculator Configuration";
let elDtPromoCalculator = $('#dt_promo_calculator');
let elMainActivity = $('#filter_mainActivity');
let elChannel = $('#filter_channel');
let txt_mainActivity, txt_channel;
let dt_promo_calculator;
heightContainer = 280;


$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListFilter();

    dt_promo_calculator = elDtPromoCalculator.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/configuration/promo-calculator/list',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'mainActivityId',
                width: 20,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full) {
                    return `<a class="btn btn-icon btn-sm btn-optima btn-clean h-20px" href="/configuration/promo-calculator/form?m=update&i=${data}&c=${full['channelId']}" title="Edit Data"><i class="fa fa-edit text-optima"></i></a>`
                }
            },
            {
                targets: 1,
                data: 'mainActivity',
                title: 'Main Activity',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                data: 'channelLongDesc',
                title: 'Channel',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                data: 'baseline',
                title: 'Baseline',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                data: 'uplift',
                title: 'Uplift',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 5,
                data: 'totalSales',
                title: 'Total Sales',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 6,
                data: 'salesContribution',
                title: 'Sales Contribution',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 7,
                data: 'storesCoverage',
                title: 'Stores Coverage',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 8,
                data: 'redemptionRate',
                title: 'Redemption Rate',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 9,
                data: 'cr',
                title: 'CR',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 10,
                data: 'cost',
                title: 'Cost',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 11,
                data: 'baselineRecon',
                title: 'Baseline Recon',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 12,
                data: 'upliftRecon',
                title: 'Uplift Recon',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 13,
                data: 'totalSalesRecon',
                title: 'Total Sales Recon',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 14,
                data: 'salesContributionRecon',
                title: 'Sales Contribution Recon',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 15,
                data: 'storesCoverageRecon',
                title: 'Stores Coverage Recon',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 16,
                data: 'redemptionRateRecon',
                title: 'Redemption Rate Recon',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 17,
                data: 'crRecon',
                title: 'CR Recon',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 18,
                data: 'costRecon',
                title: 'Cost Recon',
                className: 'align-middle text-nowrap',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        let strMessage = e.jqXHR['responseJSON']['message']
        if(strMessage === "") strMessage = "Please contact your vendor"
        Swal.fire({
            text: strMessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}

        });
    };

    $('#btn_create').on('click', function() {
        checkFormAccess('create_rec', '', '/configuration/promo-calculator/form', '')
    });

    $('#btn_mass_upload').on('click', function() {
        checkFormAccess('create_rec', '', '/configuration/promo-calculator/form-upload', '')
    });
});

$('#dt_promo_calculator_search').on('keyup', function () {
    dt_promo_calculator.search(this.value).draw();
});

elMainActivity.on('change', function () {
    let data = $(this).select2('data');
    if (data.length > 0) {
        if (data[0].id !== "") {
            txt_mainActivity = data[0].text
        } else {
            txt_mainActivity ='ALL'
        }
    } else {
        txt_mainActivity ='ALL'
    }
});

elChannel.on('change', function () {
    let data = $(this).select2('data');
    if (data.length > 0) {
        if (data[0].id !== "") {
            txt_channel = data[0].text
        } else {
            txt_channel ='ALL'
        }
    } else {
        txt_channel ='ALL'
    }
});

$('#dt_promo_calculator_view').on('click', function (){
    let btn = document.getElementById('dt_promo_calculator_view');
    let filter_mainActivity = (elMainActivity.val() ?? 0);
    let filter_channel = (elChannel.val() ?? 0);

    let url = "/configuration/promo-calculator/list?mainActivityId=" + filter_mainActivity + "&channelId=" + filter_channel;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_calculator.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        console.log(xhr)
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});


$('#btn_export_excel').on('click', function() {
    let filter_mainActivity = ((elMainActivity.val() === undefined || elMainActivity.val() === '') ? 0 : elMainActivity.val());
    let filter_channel = ((elChannel.val() === undefined || elChannel.val() === '') ? 0 : elChannel.val());
    let p_mainActivity = (filter_mainActivity === 0 || filter_mainActivity === undefined) ? 'ALL' : txt_mainActivity;
    let p_channel = (filter_channel === 0 || filter_channel === undefined) ? 'ALL' : txt_channel;

    let url = "/configuration/promo-calculator/export-xls?mainActivityId=" + filter_mainActivity + "&channelId=" + filter_channel + '&mainActivity=' + p_mainActivity + '&channel=' + p_channel;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const getListFilter = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/promo-calculator/filter",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let dataMainActivity = [];
                let dataChannel = [];
                for (let j = 0, len = result.data.mainActivity.length; j < len; ++j){
                    dataMainActivity.push({
                        id: result.data.mainActivity[j].mainActivityId,
                        text: result.data.mainActivity[j].mainActivityDesc
                    });
                }
                $('#filter_mainActivity').select2({
                    placeholder: "Select an Main Activity",
                    width: '100%',
                    data: dataMainActivity
                });
                for (let j = 0, len = result.data.channel.length; j < len; ++j){
                    dataChannel.push({
                        id: result.data.channel[j].channelId,
                        text: result.data.channel[j].channelLongDesc
                    });
                }
                $('#filter_channel').select2({
                    placeholder: "Select an Main Activity",
                    width: '100%',
                    data: dataChannel
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}
