'use strict';

let dt_promo_planning, elDtPromoPlanning = $('#dt_promo_planning');
let swalTitle = "Promo Planning";
heightContainer = 320;
let dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    if (localStorage.getItem('promoPlanningState')) {
        dataFilter = JSON.parse(localStorage.getItem('promoPlanningState'));

        url_datatable = '/promo/planning/list/paginate/filter?period=' + dataFilter.period +
            "&startFrom=" + dataFilter.activityStart + "&startTo=" + dataFilter.activityEnd +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId;
    } else {
        url_datatable = '/promo/planning/list/paginate/filter?period=' + $('#filter_period').val() + "&startFrom=" + $('#filter_activity_start').val() + "&startTo=" + $('#filter_activity_end').val();
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListEntity()]).then(async function () {
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
            $('#filter_activity_start').flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(dataFilter.activityStart)
            });
            $('#filter_activity_end').flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(dataFilter.activityEnd)
            });
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_promo_planning = elDtPromoPlanning.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        saveState: true,
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'promoPlanId',
                width: 20,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full) {
                    let cancel, createPromo, popMenu
                    if (full.lastStatus) {
                        if (full.lastStatus.substring(0, 9) === 'Cancelled') {
                            cancel = ''
                        } else {
                            cancel = '<a class="dropdown-item text-start cancel-record" href="#"><i class="fa fa-undo fs-6"></i> Cancel Planning</a>'
                        }
                    }
                    if (full.tsCode !== "" && full.tsCode != null) {
                        createPromo = '<a class="dropdown-item text-start create-promo" href="/promo/creation/form?method=createPromo&promoPlanId=' + full.promoPlanId + '&c='+ full['categoryShortDesc'] +'"><i class="fa fa-paper-plane fs-6"></i> Create Promo</a>';
                    } else {
                        createPromo = '<a class="dropdown-item text-start edit-record" href="/promo/planning/form?method=update&promoPlanId=' + full.promoPlanId + '"><i class="fa fa-edit fs-6"></i> Edit Data</a>';
                    }

                    if (full.promoId !== 0 || full.lastStatus.substring(0, 9) === 'Cancelled') {
                        popMenu = '\
                            <div class="me-0">\
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                </a>\
                                <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                    <a class="dropdown-item text-start duplicate-record" href="/promo/planning/form?method=duplicate&promoPlanId=' + full.promoPlanId + '"><i class="fa fa-file-archive fs-6"></i> Duplicate Data</a>\
                                </div>\
                            </div>\
                        ';
                    } else {
                        popMenu = '\
                            <div class="me-0">\
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                </a>\
                                <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                    <a class="dropdown-item text-start duplicate-record" href="/promo/planning/form?method=duplicate&promoPlanId=' + full.promoPlanId + '"><i class="fa fa-copy fs-6"></i> Duplicate Data</a>\
                                    ' + cancel + '\
                                    ' + createPromo + '\
                                </div>\
                            </div>\
                        ';
                    }
                    return popMenu;
                }
            },
            {
                targets: 1,
                data: 'refId',
                width: 120,
                title: 'Planning ID',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'TSCode',
                data: 'tsCode',
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Entity',
                data: 'entityShortDesc',
                width: 150,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'Distributor',
                data: 'distributorShortDesc',
                width: 150,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 5,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 150,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 6,
                title: 'Sub Brand',
                data: 'brandDesc',
                width: 150,
                className: 'align-middle',
                createdCell: function (cell, data) {
                    let $cell = $(cell);

                    if (data) {
                        $(cell).contents().wrapAll('<div class="content"></div>');
                        let $content = $cell.find(".content");
                        if (data.length > 45)
                            $(cell).append($('<button class="btn btn-clean-more">Read more</button>'));
                        let $btn = $(cell).find("button");

                        $content.css({
                            "padding": "0",
                            "height": "20px",
                            "width": "300px",
                            "overflow": "hidden"
                        })
                        $cell.data("isLess", true);

                        $btn.click(function () {
                            let isLess = $cell.data("isLess");
                            $content.css("height", isLess ? "auto" : "20px")
                            $(this).text(isLess ? "Read less" : "Read more")
                            $cell.data("isLess", !isLess)
                        })
                    }
                }
            },
            {
                targets: 7,
                title: 'Mechanism',
                data: 'mechanism',
                width: 250,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 8,
                title: 'Promo Start',
                data: 'startPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 9,
                title: 'Promo End',
                data: 'endPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 10,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 250,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 11,
                title: 'Investment',
                data: 'investment',
                width: 100,
                className: 'align-middle text-end',
                render: function (data) {
                    return formatMoney(data, 0, ".", ",");
                }
            },
            {
                targets: 12,
                title: 'Last Status',
                data: 'lastStatus',
                width: 150,
                className: 'align-middle text-nowrap'
            },
            {
                targets: 13,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 150,
                className: 'align-middle text-nowrap'
            },
            {
                targets: 14,
                title: 'Cancel Notes',
                data: 'cancelNotes',
                width: 200,
                className: 'align-middle text-nowrap'
            },
            {
                targets: 15,
                title: 'Initiator Notes',
                data: 'initiator_notes',
                width: 200,
                className: 'align-middle text-nowrap'
            },
            {
                targets: 16,
                title: 'TSCode On',
                data: 'tsCodeOn',
                width: 120,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    if (data) {
                        return formatDateTime(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 17,
                title: 'TSCode By',
                data: 'tsCodeBy',
                width: 80,
                className: 'align-middle text-nowrap'
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function () {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        let strMessage = e.jqXHR.responseJSON.message
        if (strMessage === "") strMessage = "Please contact your vendor"
        Swal.fire({
            text: strMessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            allowOutsideClick: false,
            allowEscapeKey: false,
        });
    };

    elDtPromoPlanning.on('click', '.cancel-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_promo_planning.row(tr).data();
        let promoPlanningId = parseInt(trdata.promoPlanId);

        Swal.fire({
            title: 'Are you sure to cancel ' + trdata.refId + ' ?',
            text: "You won't be able to revert this",
            icon: "warning",
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Submit',
            cancelButtonText: 'Cancel',
            customClass: {
                confirmButton: "btn btn-optima",
                cancelButton: "btn btn-optima",
            },
            html:
                '<select class="form-select form-select-sm mb-2" data-control="select2" name="reason" id="reason" data-placeholder="Select a Reason" data-allow-clear="true">' +
                '   <option value="Brand plan amendment">Brand plan amendment</option>\n' +
                '   <option value="Retailers operational issue">Retailers operational issue</option>\n' +
                '   <option value="Danones management decision">Danones management decision</option>\n' +
                '   <option value="Miscalculation or human error">Miscalculation or human error</option>\n' +
                '   <option value="Others">Others</option>\n' +
                '</select>\n' +
                '<input type="text" id="reason-text" class="form-control form-control-sm d-none">',
            didOpen: () => {
                $('#reason').on('change', function () {
                    if ($('#reason').val() === "Others") {
                        $('#reason-text').removeClass('d-none');
                    } else {
                        $('#reason-text').addClass('d-none');
                    }
                });
            },
        }).then(function (result) {
            if (result.value) {
                let reason;
                let elReason= $('#reason');
                if (elReason.val() === "Others") {
                    reason = $('#reason-text').val();
                } else {
                    reason = elReason.val();
                }
                $.ajax({
                    url: '/promo/planning/cancel',
                    type: "POST",
                    data: {promoPlanningId: promoPlanningId, reason: reason},
                    dataType: "JSON",
                    async: false,
                    success: function (result) {
                        if (!result.error) {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "success",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function () {
                                window.location.href = '/promo/planning';
                            });
                        } else {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "error",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            });
                        }
                    },
                    error: function (jqXHR) {
                        console.log(jqXHR);
                    }
                });
            }
        });
    });
});

$('#filter_period').on('blur', function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

$('#filter_activity_start').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_end.val(el_start.val());
        el_end.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(startDate)
        });
    }
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
});

$('#filter_period').on('change', function () {
    let period = this.value;
    let startDate = formatDate(new Date(period, 0, 1));
    let endDate = formatDate(new Date(period, 11, 31));
    $('#filter_activity_start').val(startDate);
    $('#filter_activity_end').val(endDate);
    $('#filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    let elDistributor = $('#filter_distributor');
    elDistributor.empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    elDistributor.val('').trigger('change');
});

$('#dt_promo_planning_search').on('keyup', function () {
    dt_promo_planning.search(this.value).draw();
});

$('#dt_promo_planning_view').on('click', function () {
    let btn = document.getElementById('dt_promo_planning_view');
    let filter_period = ($('#filter_period').val() ?? "");
    let filter_entity = ($('#filter_entity').val() ?? "");
    let filter_distributor = ($('#filter_distributor').val() ?? "");
    let filter_activity_start = ($('#filter_activity_start').val() ?? "");
    let filter_activity_end = ($('#filter_activity_end').val() ?? "");
    let url = "/promo/planning/list/paginate/filter?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_planning.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        let data_filter = {
            period: $('#filter_period').val(),
            activityStart: $('#filter_activity_start').val(),
            activityEnd: $('#filter_activity_end').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? ""),
        };

        localStorage.setItem('promoPlanningState', JSON.stringify(data_filter));
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_create').on('click', function () {
    checkFormAccess('create_rec', '', '/promo/planning/form', '')
});

$("#dt_promo_planning_download_template").on('click', function () {
    let url = file_host + '/assets/media/templates/Planning Upload Template v.1.2.xlsx';
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$("#dt_promo_planning_upload").on('click', function () {
    let elRowUpload = $('#row_upload');
    if (elRowUpload.hasClass('d-none')) {
        elRowUpload.removeClass('d-none');
    } else {
        elRowUpload.addClass('d-none');
    }
});

$('#btn_upload').on('click', function () {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;

            let formData = new FormData($('#form_budget')[0]);
            let url = '/promo/planning/upload-xls';
            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                },
                success: function (result) {
                    if (!result.error) {
                        Swal.fire({
                            title: 'Files Uploaded!',
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            $("#card_budget").removeClass("d-none");
                            dt_budget.clear().draw();
                            dt_budget.rows.add(result.data).draw();
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                },
                complete: function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                },
                error: function (jqXHR) {
                    console.log(jqXHR.message)
                    Swal.fire({
                        title: swalTitle,
                        text: "Files Upload Failed",
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        } else {
            Swal.fire({
                title: swalTitle,
                text: 'Choose file...',
                icon: "warning",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

$('#btn_export_excel').on('click', function () {
    let text_entity;
    let elEntity = $('#filter_entity');
    let elDistributor = $('#filter_distributor');
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

    let filter_period = ($('#filter_period').val() ?? "");
    let filter_entity = (elEntity.val() ?? "0");
    let filter_distributor = (elDistributor.val() ?? "0");
    let filter_activity_start = ($('#filter_activity_start').val() ?? "");
    let filter_activity_end = ($('#filter_activity_end').val() ?? "");
    let url = "/promo/planning/export-xls?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity + "&distributor=" + text_distributor;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const checkFormAccessManual = (menuId, trxAction, id, url, popMsg, confirmButtonText = "Yes, delete it", type = "delete") => {
    $.ajax({
        url: "/promo/planning/check-form-access",
        type: "GET",
        data: {menuid: menuId, access_name: trxAction},
        dataType: "JSON",
        async: true,
        success: function (result) {
            if (!result.error) {
                switch(trxAction) {
                    case "create_rec":
                        window.location.href = url;
                        break;
                    case "update_rec":
                        window.location.href = url;
                        break;
                    case "delete_rec":
                        Swal.fire({
                            title: swalTitle,
                            text: popMsg,
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#AAAAAA',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            cancelButtonText: 'No, cancel',
                            confirmButtonText: confirmButtonText,
                            reverseButtons: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                if(type === "activate"){
                                    fActivateRecord(id);
                                } else if (type === 'deactivate'){
                                    fDeactivateRecord(id);
                                } else {
                                    fDeleteRecord(id);
                                }
                            }
                        })
                        break;
                    default:
                    // code block
                }
            } else {
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }

        },
        error: function (jqXHR) {
            if (jqXHR.status === 403) {
                Swal.fire({
                    title: swalTitle,
                    text: jqXHR.responseJSON.message,
                    icon: "warning",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: { confirmButton: "btn btn-optima" }
                }).then(function () {
                    window.location.href = '/login-page';
                });
            }
            console.log(jqXHR);
        },
    });
}

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/planning/list/entity",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
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
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
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
            url: "/promo/planning/list/distributor",
            type: "GET",
            dataType: 'json',
            data: {entityId: entityId},
            async: true,
            success: function (result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
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
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}
