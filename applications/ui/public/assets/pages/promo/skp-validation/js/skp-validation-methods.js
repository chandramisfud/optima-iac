'use strict';

let dt_skp_validation;
let swalTitle = "SKP Validation";
heightContainer = 320;
let dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });


    let filter_activity_start = $('#filter_activity_start');
    let filter_activity_end = $('#filter_activity_end');

    if (localStorage.getItem('promoSKPValidationState')) {
        dataFilter = JSON.parse(localStorage.getItem('promoSKPValidationState'));

        url_datatable = '/promo/skp-validation/list?period=' + dataFilter.period +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId + "&startFrom=" + dataFilter.activityStart + "&startTo=" + dataFilter.activityEnd;
    } else {
        url_datatable = '/promo/skp-validation/list?period=' + $('#filter_period').val() + "&startFrom=" + filter_activity_start.val() + "&startTo=" + filter_activity_end.val();
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListEntity(), getListChannel()]).then(async function () {
        getSKPStatus();
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));

            filter_activity_start.flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(dataFilter.activityStart)
            }).setDate(new Date(dataFilter.activityStart));
            filter_activity_start.next().css('background-color', '#fff !important');

            filter_activity_end.flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(dataFilter.activityEnd)
            }).setDate(new Date(dataFilter.activityEnd));
            filter_activity_end.next().css('background-color', '#fff !important');

            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());

            filter_activity_start.flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(new Date().getFullYear() + '-01-01')
            }).setDate(new Date(new Date().getFullYear()+ "-01-01"));

            filter_activity_end.flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(new Date().getFullYear() + '-12-31')
            });

        }
        filter_activity_start.next().css('background-color', '#fff !important');
        filter_activity_end.next().css('background-color', '#fff !important');
    });

    dt_skp_validation = $('#dt_skp_validation').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        saveState: false,
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
                width: 20,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-bs-placement="right" >\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start validate-record" href="/promo/skp-validation/form?method=update&id=' + full.promoId + '&c=' + full.categoryShortDesc + '"><i class="fa fa-edit fs-6"></i> Validate</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                data: 'skpstatus',
                width: 80,
                title: 'SKP Status',
                className: 'align-middle text-nowrap',
                render: function (data) {
                    switch (data) {
                        case 0:
                            return 'New'
                        case 1:
                            return 'Pending'

                        default:
                            return 'Final'
                    }
                }
            },
            {
                targets: 2,
                title: 'Promo ID',
                data: 'refId',
                width: 250,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 250,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'Initiator',
                data: 'createBy',
                width: 250,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 5,
                title: 'Initiator Name',
                data: 'createdName',
                width: 250,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 6,
                title: 'Channel',
                data: 'channelDesc',
                width: 200,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 7,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 200,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 8,
                title: 'Notes',
                data: 'skp_notes',
                width: 200,
                className: 'align-middle text-nowrap'
            },
        ],
        initComplete: function() {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}

        });
    };
});

$('#dt_skp_validation_search').on('keyup', function () {
    dt_skp_validation.search(this.value, false, false).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    let elDistributor = $('#filter_distributor');
    elDistributor.empty();
    if ($(this).val()) await getListDistributor($(this).val());
    blockUI.release();
    elDistributor.val('').trigger('change');
});

$('#filter_period').on('change', function() {
    let period = this.value;
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = formatDate(new Date(period,0,1));
    let endDate = formatDate(new Date(period, 11, 31));
    el_start.val(startDate);
    el_end.val(endDate);
    $('#filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });
    el_start.next().css('background-color', '#fff !important');
    el_end.next().css('background-color', '#fff !important');
});

$('#filter_activity_start').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_end.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(startDate)
        });
    }
    el_start.next().css('background-color', '#fff !important');
    el_end.next().css('background-color', '#fff !important');
});

$('#filter_activity_end').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_start.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(endDate)
        });
    }
    el_start.next().css('background-color', '#fff !important');
    el_end.next().css('background-color', '#fff !important');
});

$('#dt_skp_validation_view').on('click', function (){
    let btn = document.getElementById('dt_skp_validation_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_channel = ($('#filter_channel').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let filter_skp_status = ($('#filter_skp_status').val()) ?? "";

    let url = "/promo/skp-validation/list?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel + "&SKPstatus=" + filter_skp_status;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_skp_validation.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        let data_filter = {
            period: $('#filter_period').val(),
            activityStart: $('#filter_activity_start').val(),
            activityEnd: $('#filter_activity_end').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? ""),
        };

        localStorage.setItem('promoSKPValidationState', JSON.stringify(data_filter));
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$("#dt_skp_validation").on('click', '.validate-record', function () {
    let tr = this.closest("tr");
    let trdata = dt_skp_validation.row(tr).data();

    window.location.href = '/promo/skp-validation/form?method=update&id=' + trdata.promoId + '&c=' + trdata.categoryShortDesc;
});

$('#btn_export_excel').on('click', function() {
    let text_entity;
    let text_channel;
    let elEntity = $('#filter_entity');
    let elDistributor = $('#filter_distributor');
    let elChannel = $('#filter_channel');

    let data = elEntity.select2('data');
    (data[0].id !== "") ? text_entity = data[0].text : text_entity ='ALL';
    let text_distributor;
    let dataDist = elDistributor.select2('data');
    if (dataDist.length > 0) {
        if (dataDist[0].id !== "") {
            text_distributor = dataDist[0].text
        } else {
            text_distributor ='ALL'
        }
    } else {
        text_distributor ='ALL'
    }
    let dataChannel = elChannel.select2('data');
    (dataChannel[0].id !== "") ? text_channel = dataChannel[0].text : text_channel ='ALL';

    let filter_period = ($('#filter_period').val() ?? "");
    let filter_entity = (elEntity.val() ?? "0");
    let filter_distributor = (elDistributor.val() ?? "0");
    let filter_channel = (elChannel.val() ?? "0");
    let filter_activity_start = ($('#filter_activity_start').val() ?? "");
    let filter_activity_end = ($('#filter_activity_end').val() ?? "");
    let filter_skp_status = ($('#filter_skp_status').val() ?? "");

    let url = "/promo/skp-validation/export-xls?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel + "&SKPstatus=" + filter_skp_status
        + "&entity=" + text_entity + "&distributor=" + text_distributor + "&channel=" + text_channel;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/skp-validation/list/entity",
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/skp-validation/list/distributor",
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/skp-validation/list/channel",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_channel').select2({
                    placeholder: "Select a Channel",
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSKPStatus = () => {
    let data = [
        {
            id: '0',
            text: 'New'
        },{
            id: '1',
            text: 'Pending'
        },{
            id: '2',
            text: 'Final'
        }
    ];
    $('#filter_skp_status').select2({
        placeholder: "Select a SKP Status",
        width: '100%',
        data: data
    });
}
