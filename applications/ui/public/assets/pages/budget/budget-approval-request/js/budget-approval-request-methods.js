'use strict';

let dialerObject;
let swalTitle = 'Budget Mass Approval';
let elSelectCategory = $('#filter_category');
let elSelectChannel = $('#filter_channel');
let elSelectGroupBrand = $('#filter_group_brand');
let elSelectStatus = $('#filter_status');
let elSelectMonth = $('#filter_month');
let elDtBudgetApprovalRequestSummary = $('#dt_budget_approval_request_summary');
let elDtBudgetApprovalRequestDetail = $('#dt_budget_approval_request_detail');
let dt_budget_approval_request_summary, dt_budget_approval_request_detail;
let validator;
heightContainer = 315;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        min: 2025,
        step: 1,
    });

    validator =  FormValidation.formValidation(document.getElementById('printPDF'), {
        fields: {
            batchId: {
                validators: {
                    notEmpty: {
                        message: "Batch Id must be enter"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
        }
    });

    let filter_period = $('#filter_period').val();
    let filter_month = JSON.stringify(elSelectMonth.val());
    let filter_channel = JSON.stringify(elSelectChannel.val());
    let filter_group_brand = JSON.stringify(elSelectGroupBrand.val());
    let filter_five_bio = $('#filter_five_bio').val();
    let filter_status = JSON.stringify(elSelectStatus.val());

    dt_budget_approval_request_summary = elDtBudgetApprovalRequestSummary.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-12'i>>",
        ajax: {
            url: `/budget/approval-request/list-summary?period=${filter_period}&month=${filter_month}&channelId=${filter_channel}&groupBrand=${filter_group_brand}&is5Bio=${filter_five_bio}&budgetApprovalStatus=${filter_status}`,
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Channel',
                data: 'channel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Sub Activity Type',
                data: 'subActivityType',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Total Cost',
                data: 'totInvestment',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return formatMoney(0, 0);
                    }
                }
            },
        ],
        initComplete: function () {

        },
        drawCallback: function () {

        },
    });

    dt_budget_approval_request_detail = elDtBudgetApprovalRequestDetail.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-2'l><'col-sm-4'i><'col-sm-6'p>>",
        ajax: {
            url: `/budget/approval-request/list-detail?period=${filter_period}&month=${filter_month}&channelId=${filter_channel}&groupBrand=${filter_group_brand}&is5Bio=${filter_five_bio}&budgetApprovalStatus=${filter_status}`,
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[1, 'ASC']],
        language: {
            infoFiltered: '',
        },
        columnDefs: [
            {
                targets: 0,
                title: 'Period',
                data: 'period',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'promoId',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Status',
                data: 'statusDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Activity',
                data: 'activity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Activity Name',
                data: 'activityName',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Channel',
                data: 'channel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Account',
                data: 'account',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Sub Account',
                data: 'subAccount',
                className: 'text-nowrap align-middle',
                render: function (data) {
                    return data ?? '';
                }
            },
            {
                targets: 8,
                title: 'Promo Start',
                data: 'promoStart',
                className: 'text-nowrap align-middle',
                render: function (data) {
                    if (data) {
                        return formatDateOptima(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 9,
                title: 'Promo End',
                data: 'promoEnd',
                className: 'text-nowrap align-middle',
                render: function (data) {
                    if (data) {
                        return formatDateOptima(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 10,
                title: 'Cost',
                data: 'investment',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return formatMoney(0, 0);
                    }
                }
            },
            {
                targets: 11,
                title: 'Request Approval On',
                data: 'approvalRequestOn',
                className: 'text-nowrap align-middle',
                render: function (data) {
                    if (data) {
                        return formatDateOptima(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 12,
                title: 'Batch ID',
                data: 'batchId',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function () {

        },
        drawCallback: function () {

        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        console.log(e.jqXHR);
        let strMessage = e.jqXHR['responseJSON'].message
        if (strMessage === "") strMessage = "Please contact your vendor"
        Swal.fire({
            text: strMessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            showLoaderOnConfirm: false,
            allowOutsideClick: false,

        });
    };

    getListFilter();
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2025) {
        $(this).val("2025");
        dialerObject.setValue(2025);
    }
});

$('#dt_budget_approval_request_summary_search').on('keyup', function() {
    dt_budget_approval_request_summary.search(this.value, false, false).draw();
});

$('#dt_budget_approval_request_detail_search').on('keyup', function() {
    dt_budget_approval_request_detail.search(this.value, false, false).draw();
});

$('#btn_pdf').on('click', function () {
    $('#modal_print_pdf').modal('show');
});

$('#btn_export_pdf').on('click', function () {
    validator.validate().then(async function (status) {
        if (status === "Valid") {
            let btn = document.querySelector("#btn_export_pdf");
            let elBatchId = $('#batchId');
            let batchId = elBatchId.val();
            btn.setAttribute("data-kt-indicator", "on");
            btn.disabled = !0;
            let res = await checkBatchId(batchId);
            if (res.error) {
                btn.setAttribute("data-kt-indicator", "off");
                btn.disabled = !1;
                return Swal.fire({
                    title: 'Print PDF',
                    text: res.message,
                    icon: "warning",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
            btn.setAttribute("data-kt-indicator", "off");
            btn.disabled = !1;
            if (res['data']['period']['category'] === "RC") {
                if (res['data']['period']['above5Bio']) {
                    let hiddenIFrameID = 'hiddenDownloaderAbove';
                    let iframe = document.createElement('iframe');
                    iframe.id = hiddenIFrameID;
                    iframe.style.display = 'none';
                    document.body.appendChild(iframe);
                    iframe.src = `/budget/approval-request/download-pdf-above?batchId=${batchId}`;
                }

                if (res['data']['period']['under5Bio']) {
                    let hiddenIFrameID = 'hiddenDownloaderAbove';
                    let iframe = document.createElement('iframe');
                    iframe.id = hiddenIFrameID;
                    iframe.style.display = 'none';
                    document.body.appendChild(iframe);
                    iframe.src = `/budget/approval-request/download-pdf-below?batchId=${batchId}`;
                }
            }
            if (res['data']['period']['category'] === "DC") {
                if (res['data']['period']['above5Bio']) {
                    let hiddenIFrameID = 'hiddenDownloaderAbove';
                    let iframe = document.createElement('iframe');
                    iframe.id = hiddenIFrameID;
                    iframe.style.display = 'none';
                    document.body.appendChild(iframe);
                    iframe.src = `/budget/approval-request/download-pdf-above?batchId=${batchId}`;
                }

                if (res['data']['period']['under5Bio']) {
                    let hiddenIFrameID = 'hiddenDownloaderAbove';
                    let iframe = document.createElement('iframe');
                    iframe.id = hiddenIFrameID;
                    iframe.style.display = 'none';
                    document.body.appendChild(iframe);
                    iframe.src = `/budget/approval-request/download-pdf-below?batchId=${batchId}&category=DC`;
                }
            }
        }
    })
});

$('#modal_print_pdf').on('hide.bs.modal', function () {
    $('#batchId').val('');
    validator.resetForm('printPDF');
});

$('#btn_view').on('click', function () {
    let btn = document.getElementById('btn_view');
    let filter_period = $('#filter_period').val();
    let filter_month = JSON.stringify(elSelectMonth.val());
    let filter_channel = JSON.stringify(elSelectChannel.val());
    let filter_group_brand = JSON.stringify(elSelectGroupBrand.val());
    let filter_five_bio = $('#filter_five_bio').val();
    let filter_status = JSON.stringify(elSelectStatus.val());
    let filter_category = JSON.stringify(elSelectCategory.val());

    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    let urlSummary = `/budget/approval-request/list-summary?period=${filter_period}&month=${filter_month}&channelId=${filter_channel}&groupBrand=${filter_group_brand}&is5Bio=${filter_five_bio}&budgetApprovalStatus=${filter_status}&categoryId=${filter_category}`;
    let url = `/budget/approval-request/list-detail?period=${filter_period}&month=${filter_month}&channelId=${filter_channel}&groupBrand=${filter_group_brand}&is5Bio=${filter_five_bio}&budgetApprovalStatus=${filter_status}&categoryId=${filter_category}`;
    dt_budget_approval_request_summary.ajax.url(urlSummary).load();
    dt_budget_approval_request_detail.ajax.url(url).load(async function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function () {
    let filter_period = $('#filter_period').val();
    let filter_month = JSON.stringify(elSelectMonth.val());
    let filter_channel = JSON.stringify(elSelectChannel.val());
    let filter_group_brand = JSON.stringify(elSelectGroupBrand.val());
    let filter_status = JSON.stringify(elSelectStatus.val());
    let filter_category = JSON.stringify(elSelectCategory.val());
    let filter_five_bio = $('#filter_five_bio').val();
    window.open(`/budget/approval-request/download-excel?period=${filter_period}&month=${filter_month}&month=${filter_month}&channelId=${filter_channel}&groupBrand=${filter_group_brand}&budgetApprovalStatus=${filter_status}&categoryId=${filter_category}&fiveBio=${filter_five_bio}`, '_blank');
});

$('.btn_send_email').on('click', function () {
    let categoryId = $(this).attr('data-category');
    let categoryDesc = $(this).attr('data-category-desc');
    let e = document.querySelector("#btn_send_email");
    Swal.fire({
        title: swalTitle,
        text: `Mass approval email will be send to approver. Confirm?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#AAAAAA',
        allowOutsideClick: false,
        allowEscapeKey: false,
        cancelButtonText: 'No, cancel',
        confirmButtonText: 'Yes',
        reverseButtons: true
    }).then((result) => {
        if(result.isConfirmed){
            let filter_period = $('#filter_period').val();
            let filter_month = JSON.stringify(elSelectMonth.val());
            let filter_channel = JSON.stringify(elSelectChannel.val());
            let filter_group_brand = JSON.stringify(elSelectGroupBrand.val());
            let filter_five_bio = $('#filter_five_bio').val();
            let formData = new FormData();
            formData.append('period', filter_period);
            formData.append('month', filter_month);
            formData.append('channelId', filter_channel);
            formData.append('groupBrand', filter_group_brand);
            formData.append('categoryId', categoryId);
            formData.append('categoryDesc', categoryDesc);
            formData.append('fiveBio', filter_five_bio);
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            $.get('/refresh-csrf').done(function(data) {
                let elMeta = $('meta[name="csrf-token"]');
                elMeta.attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': elMeta.attr('content')
                    }
                });
                $.ajax({
                    url: '/budget/approval-request/send',
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
                                title: swalTitle,
                                text: result.message,
                                icon: "success",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(async function () {
                                dt_budget_approval_request_summary.ajax.reload();
                                dt_budget_approval_request_detail.ajax.reload();
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
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown)
                        Swal.fire({
                            title: swalTitle,
                            text: "Failed to send email, an error occurred in the process",
                            icon: "error",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                });
            });
        }
    });
});

const checkBatchId = (pBatchId) => {
    return new Promise((resolve) => {
        $.ajax({
            url         : "/budget/approval-request/check-batch",
            type        : "GET",
            dataType    : 'json',
            data        : {batchId: pBatchId},
            async       : true,
            success: function(result) {
                return resolve(result)
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return resolve({error: true, message: errorThrown});
            }
        });
    });
}

const getListFilter = () => {
    $.ajax({
        url         : "/budget/approval-request/list/filter",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            if (!result['error']) {
                let data = result['data'];
                let categoryList = data['category'];
                let channelList = data['channel'];
                let groupBrandList = data['grpBrand'];
                let approvalStatus = data['approvalStatus'];
                let month = data['months'];

                //<editor-fold desc="Dropdown Channel">
                let dataCategoryList = [];
                for (let i = 0, len = categoryList.length; i < len; ++i) {
                    dataCategoryList.push({
                        id: categoryList[i]['categoryId'],
                        text: categoryList[i]['categoryDesc']
                    });
                }

                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectCategory.select2({
                        placeholder: 'Select a Category',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectCategory.val() || []).length;
                            let total = $('option', elSelectCategory).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataCategoryList
                    });

                    $('[aria-controls="select2-filter_category-container"]').addClass('form-select form-select-sm');
                });
                //</editor-fold>

                //<editor-fold desc="Dropdown Channel">
                let dataChannelList = [];
                for (let i = 0, len = channelList.length; i < len; ++i) {
                    dataChannelList.push({
                        id: channelList[i]['id'],
                        text: channelList[i]['longDesc']
                    });
                }

                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectChannel.select2({
                        placeholder: 'Select a Channel',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectChannel.val() || []).length;
                            let total = $('option', elSelectChannel).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataChannelList
                    });

                    $('[aria-controls="select2-filter_channel-container"]').addClass('form-select form-select-sm');
                });
                //</editor-fold>

                //<editor-fold desc="Dropdown Group Brand">
                let dataGroupBrandList = [];
                for (let i = 0, len = groupBrandList.length; i < len; ++i) {
                    dataGroupBrandList.push({
                        id: groupBrandList[i]['groupBrandId'],
                        text: groupBrandList[i]['groupBrand']
                    });
                }
                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectGroupBrand.select2({
                        placeholder: 'Select a Brand',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectGroupBrand.val() || []).length;
                            let total = $('option', elSelectGroupBrand).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataGroupBrandList
                    });

                    $('[aria-controls="select2-filter_group_brand-container"]').addClass('form-select form-select-sm');
                });
                //</editor-fold>

                //<editor-fold desc="Dropdown Status">
                let dataStatusList = [];
                for (let i = 0, len = approvalStatus.length; i < len; ++i) {
                    dataStatusList.push({
                        id: approvalStatus[i]['StatusCode'],
                        text: approvalStatus[i]['statusDesc']
                    });
                }
                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectStatus.select2({
                        placeholder: 'Select a Status',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectStatus.val() || []).length;
                            let total = $('option', elSelectStatus).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataStatusList
                    });

                    $('[aria-controls="select2-filter_status-container"]').addClass('form-select form-select-sm');
                });
                //</editor-fold>

                //<editor-fold desc="Month">
                let dataMonthList = [];
                for (let i = 0, len = month.length; i < len; ++i) {
                    dataMonthList.push({
                        id: month[i]['value'],
                        text: month[i]['text']
                    });
                }
                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectMonth.select2({
                        placeholder: 'Select a Month',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectMonth.val() || []).length;
                            let total = $('option', elSelectMonth).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataMonthList
                    });

                    $('[aria-controls="select2-filter_month-container"]').addClass('form-select form-select-sm');
                });
                //</editor-fold>

                for (let i=0; i<categoryList.length; i++) {
                    if (categoryList[i]['categoryDesc'] === "Retailer Cost") {
                        $('#btn_export_excel_rc').attr('data-category', categoryList[i]['categoryId']);
                        $('#btn_view_rc').attr('data-category', categoryList[i]['categoryId']);
                        $('#btn_send_email_rc').attr('data-category', categoryList[i]['categoryId']).attr('data-category-desc', 'RC');
                    }

                    if (categoryList[i]['categoryDesc'] === "Distributor Cost") {
                        $('#btn_export_excel_dc').attr('data-category', categoryList[i]['categoryId']);
                        $('#btn_view_dc').attr('data-category', categoryList[i]['categoryId']);
                        $('#btn_send_email_dc').attr('data-category', categoryList[i]['categoryId']).attr('data-category-desc', 'DC');

                    }
                }
            }
        },
        complete: function() {

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(errorThrown);
        }
    });
}

const downloadURL = (url, uuid) => {
    let hiddenIFrameID = 'hiddenDownloader' + uuid;
    let iframe = document.createElement('iframe');
    iframe.id = hiddenIFrameID;
    iframe.style.display = 'none';
    document.body.appendChild(iframe);
    iframe.src = url;
}
