'use strict';

var dt_vat_expired;
var el_dt_vat_expired = $('#dt_vat_expired');
var swalTitle = "VAT Expired";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListEntity();

    dt_vat_expired = el_dt_vat_expired.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/vat-expired/list',
            type: 'get',
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
                data: 'vatExpired',
                width: 20,
                orderable: false,
                title: 'VAT Expired',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    let checked = ((full.dnvatExpired === true) ? "checked" : "" );
                    return '\
                        <span class="form-check form-switch form-check-custom form-check-success form-check-solid justify-content-center">\
                            <label><input type="checkbox" class="form-check-input vat-expired" ' + checked + ' data-toggle="toggle">\
                            </label>\
                        </span>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'DN Number',
                data: 'refId',
                width: 120,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 120,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'DN Description',
                data: 'activityDesc',
                width: 300,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'DPP',
                data: 'dpp',
                width: 80,
                className: 'align-middle text-end text-nowrap',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 5,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'align-middle text-nowrap',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    el_dt_vat_expired.on('change', 'tbody td .vat-expired', function () {
        let set = $(this).closest($(this).parents('tr')).find('td .vat-expired');
        let checked = $(this).is(':checked');

        let data = dt_vat_expired.row($(this).parents('tr')).data();

        let Id = data.id;
        let url = "/dn/vat-expired/update";
        let setVATExpired = ((checked) ? 1 : 0);

        let formData = new FormData();
        formData.append('id', Id);
        formData.append('VATExpired', setVATExpired.toString());

        $.ajax({
            url         : url,
            data        : formData,
            type        : 'POST',
            async       : true,
            dataType    : 'JSON',
            cache       : false,
            contentType : false,
            processData : false,
            beforeSend: function() {
                set.disabled = !0;
            },
            success: function(result, status, xhr, $form) {
                if (!result.error) {
                    set.prop('checked', setVATExpired)
                } else {
                    if (setVATExpired === 1){
                        set.prop('checked', 0)
                    } else {
                        set.prop('checked', 1)
                    }
                }
            },
            complete: function() {
                set.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.message)
                Swal.fire({
                    title: swalTitle,
                    text: "Failed to update VAT Expired, an error occurred in the process",
                    icon: "error",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor";
        console.log(e);
        Swal.fire({
            text: "Please contact your vendor",
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
});

$('#dt_vat_expired_search').on('keyup', function() {
    dt_vat_expired.search(this.value).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_vat_expired_view').on('click', function (){
    let btn = document.getElementById('dt_vat_expired_view');
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/dn/vat-expired/list?entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_vat_expired.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/vat-expired/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].shortDesc + " - " + result.data[j].longDesc,
                        longDesc: result.data[j].longDesc
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

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/vat-expired/list/distributor",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_distributor').select2({
                    placeholder: "Select a Distributor",
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
