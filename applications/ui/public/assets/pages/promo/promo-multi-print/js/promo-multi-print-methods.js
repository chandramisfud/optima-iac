'use strict';

var dt_promo_multi_print;
var swalTitle = "Promo Multi Print";
heightContainer = 280;
var dataFilter, url_datatable;
let dialerObject, arrChecked = [];
let elDtMultiPrintPromo = $('#dt_promo_multi_print');

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListEntity()]).then(async function () {
        $('#filter_period').val(new Date().getFullYear());
        dialerObject.setValue(new Date().getFullYear());
    });

    dt_promo_multi_print = elDtMultiPrintPromo.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-5'i><'col-sm-6'p>>",
        order: [[1, 'asc']],
        ajax: {
            url:'/promo/multi-print/list?period=' + $('#filter_period').val() + "&entityId=" + $('#filter_entity').val(),
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
                data: 'promoId',
                width: 10,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center align-middle',
                checkboxes: {
                    'selectRow': false,
                },
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off">';
                    }
                    return data;
                }
            },
            {
                targets: 1,
                data: 'refId',
                width: 170,
                title: 'Promo ID',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Last Status',
                data: 'lastStatus',
                width: 300,
                className: 'text-nowrap align-middle'
            },
            {
                targets: 3,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 200,
                className: 'text-nowrap align-middle'
            },
            {
                targets: 4,
                title: 'TS Coding',
                data: 'tsCoding',
                width: 150,
                className: 'text-nowrap align-middle'
            },
            {
                targets: 5,
                title: 'Allocation',
                data: 'allocation',
                width: 180,
                className: 'text-nowrap align-middle'
            },
        ],
        initComplete: function( settings, json ) {
            $('#dt_promo_multi_print_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_promo_multi_print.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        arrChecked.push({
                            id: data[i]['promoId'],
                            yearPromo: new Date(data[i]['startPromo']).getFullYear()
                        });
                    }
                } else {
                    arrChecked = [];
                }
            });
        },
        drawCallback: function( settings, json ) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,
        });
    };

    elDtMultiPrintPromo.on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_promo_multi_print.row(this.closest('tr')).data();
        if (this.checked) {
            arrChecked.push({
                id: rows['promoId'],
                yearPromo: new Date(rows['startPromo']).getFullYear()
            });
        } else {
            let index = arrChecked.findIndex(p => p.id === rows.promoId);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }
    });
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#filter_period').on('change', async function () {
    arrChecked = [];
});

$('#dt_promo_multi_print_search').on('keyup', function () {
    dt_promo_multi_print.search(this.value, false, false).draw();
});

$('#dt_promo_multi_print_view').on('click', function (){
    let btn = document.getElementById('dt_promo_multi_print_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = (($('#filter_entity').val() == "") ? 0 : $('#filter_entity').val());
    let url = "/promo/multi-print/list?period=" + filter_period + "&entityId=" + filter_entity ;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_multi_print.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_print').on('click', function () {
    if (arrChecked.length > 0) {
        let url = "/promo/multi-print/print-pdf?id=" + encodeURIComponent(JSON.stringify(arrChecked));

        let a = document.createElement("a");
        a.href = url;
        let evt = document.createEvent("MouseEvents");
        evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
            true, false, false, false, 0, null);
        a.dispatchEvent(evt);
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select one or more items",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/multi-print/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc,
                    });
                }
                $('#filter_entity').select2({
                    placeholder: "Select an Entity",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}
