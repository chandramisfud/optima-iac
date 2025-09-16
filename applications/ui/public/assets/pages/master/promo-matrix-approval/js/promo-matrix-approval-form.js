'use strict';

let dt_profile, dt_approver, validator, method, refId, trxindex_edit_dtl, matrixApprovalId, entityId, distributorId, channelId, subChannelId;
let dataApprover = [];
let swalTitle = "Matrix Promo Approval";
let elDtApprover = $("#dt_approver");

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        digits: 0,
        groupSeparator: ",",
    }).mask("#min_investment, #max_investment");

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    matrixApprovalId = url_str.searchParams.get("matrixapprovalid");

    blockUI.block();
    disableButtonSave();
    if (method === 'update') {
        Promise.all([ getEntity(), getCategory(), getChannel(), getInitiator() ]).then(async () => {
            await getData(matrixApprovalId);
            $('#txt_info_method').text('Edit Matrix Promo Approval ');
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ getEntity(), getCategory(), getChannel(), getInitiator() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_matrix_approval');

    validator = FormValidation.formValidation(form, {
        fields: {
            entity: {
                validators: {
                    notEmpty: {
                        message: "Entity must be enter"
                    },
                }
            },
            distributor: {
                validators: {
                    notEmpty: {
                        message: "Distributor must be enter"
                    },
                }
            },
            category: {
                validators: {
                    notEmpty: {
                        message: "Category must be enter"
                    },
                }
            },
            sub_activity_type: {
                validators: {
                    notEmpty: {
                        message: "Sub Activity Type must be enter"
                    },
                }
            },
            channel: {
                validators: {
                    notEmpty: {
                        message: "Channel must be enter"
                    },
                }
            },
            initiator: {
                validators: {
                    notEmpty: {
                        message: "Initiator must be enter"
                    },
                }
            },
            min_investment: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            max_investment: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })

    dt_profile = $('#dt_profile').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        order: [[0, 'asc']],
        ajax: {
            url: '/master/matrix/promoapproval/get-list/profile',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "35vh",
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 10,
                orderable: true,
                title: 'Master Profile',
                className: 'text-nowrap',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $('#dt_profile_search').on('keyup', function () {
        dt_profile.search(this.value).draw();
    });

    dt_approver = $('#dt_approver').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "35vh",
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 10,
                className: "text-nowrap",
                render: function () {
                        return '<button class="btn btn-icon btn-sm btn-optima btn-clean btn_edit" title="edit"><i class="fa fa-edit fs-6"></i></button>' +
                            '<button class="btn btn-icon btn-sm btn-optima btn-clean btn_delete" title="remove" ><i class="fa fa-trash fs-6"/></i></button>';
                }
            },
            {
                targets: 1,
                data: 'seqApproval',
                width: 50,
                title: 'No.',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 2,
                data: 'approver',
                width: 800,
                title: 'Approver',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $('#dt_matrix_approver_search').on('keyup', function () {
        dt_approver.search(this.value).draw();
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        console.log(message);
        let strmessage = e.jqXHR['responseJSON'].message
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
});

$('#btn_back').on('click', function() {
    window.location.href = '/master/matrix/promoapproval';
});

$('#entity').on('change', async function () {
    if (!method){
        blockUI.block();
    }
    let elDistributor = $('#distributor');
    elDistributor.empty();
    if ($(this).val() !== "") await getDistributor($(this).val());
    blockUI.release();
    elDistributor.val('').trigger('change');
});

$('#category').on('change', async function () {
    if (!method){
        blockUI.block();
    }
    let elSubActivityType = $('#sub_activity_type');
    elSubActivityType.empty();
    if ($(this).val() !== "") await getSubActivityType($(this).val());
    blockUI.release();
    elSubActivityType.val('').trigger('change');
});


$('#channel').on('change', async function () {
    if (!method){
        blockUI.block();
    }
    let elSubChannel = $('#sub_channel');
    elSubChannel.empty();
    if ($(this).val() !== "") await getSubChannel($(this).val());
    blockUI.release();
    elSubChannel.val('').trigger('change');
});

$('#dt_profile').on('dblclick','tr',function(){
    let tr = this.closest("tr");
    let trdata = dt_profile.row(tr).data();
    if (dt_approver.data().length < 5 ) {
        let data = trdata;

        let dApprover = {};
        dApprover.id = data.id;
        dApprover.approver = data.id;

        dt_approver.row.add(dApprover).draw();
    } else {
        Swal.fire({
            text: "Maximum approver is 5",
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
    }
})

$('#btn_save').on('click', function() {
    let trdata = dt_approver.rows().data();
    validator.validate().then(function (status) {
        if (status === "Valid") {
            if (trdata.length === 0) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Select one or more approver",
                    icon: "warning",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
            let e = document.querySelector("#btn_save");
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            let data_detail = []
            for (let i=0; i<trdata.length; i++) {
                data_detail.push({
                    seqApproval:  i+1,
                    approver: trdata[i]['approver'],
                });
            }
            let min_invest = parseFloat($('#min_investment').val().toString().replace(/,/g, ''));
            let max_invest = parseFloat($('#max_investment').val().toString().replace(/,/g, ''));
            let formData = new FormData($('#form_matrix_approval')[0]);
            formData.append('min_invest', min_invest.toString());
            formData.append('max_invest', max_invest.toString());
            formData.append('matrixApprover', JSON.stringify(data_detail));
            let url = '/master/matrix/promoapproval/save';
            let methodTxt = 'Save Success! <br/> Update matrix promo will be processing on new tab';
            if (method === "update") {
                formData.append('id', matrixApprovalId);
                url = '/master/matrix/promoapproval/update';
                methodTxt = 'Update Success! <br/> Update matrix promo will be processing on new tab';
            }
            $.get('/refresh-csrf').done(function(data) {
                let elMeta = $('meta[name="csrf-token"]');
                elMeta.attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': elMeta.attr('content')
                    }
                });
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
                    },
                    success: function(result) {
                        if (!result.error) {
                            Swal.fire({
                                title: swalTitle,
                                html: methodTxt,
                                icon: "success",
                                confirmButtonText: "Ok",
                                allowOutsideClick: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function () {
                                if (result['matrixId'] !== 0) {
                                    let handle = window.open('/master/matrix/promoapproval/process?i=' + result['matrixId']);
                                    window.location.href = '/master/matrix/promoapproval';
                                    handle.blur();
                                    window.focus();
                                }
                            });
                        } else {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "warning",
                                confirmButtonText: "Ok",
                                customClass: {confirmButton: "btn btn-optima"}
                            });
                        }
                    },
                    complete: function() {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown)
                        Swal.fire({
                            title: swalTitle,
                            text: "Failed to save data, an error occurred in the process",
                            icon: "error",
                            confirmButtonText: "OK",
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                });
            });
        }
    });
});

elDtApprover.on('click', '.btn_edit', async function () {
    $('#modal_edit_approver').modal('show');
    let tr = this.closest("tr");
    let trindex = dt_approver.row(tr).data();
    trxindex_edit_dtl = dt_approver.row(tr).index();
    let elProfile = $('#profile');
    elProfile.empty();
    await getProfile();
    elProfile.val(trindex.approver).trigger('change');
});

$('#btn_submit').on('click', function () {
    dt_approver.row(trxindex_edit_dtl).every(function () {
        let detail = this.data();
        let elProfile = $('#profile');
        detail['id'] = elProfile.val();
        detail['approver'] = elProfile.val();
        this.invalidate();
    })
    dt_approver.draw();
    $('#modal_edit_approver').modal('hide');
});

elDtApprover.on('click', '.btn_delete', async function () {
    let tr = this.closest("tr");
    let trindex = dt_approver.row(tr).index();
    dt_approver.row(trindex).remove().draw();

    let rowdata = dt_approver.rows().data();

    dataApprover = [];

    $.each(rowdata, function (index, rowId) {
        let matrix = {}
        matrix.seqApproval = index+1;
        matrix.approver = rowId.approver;
        dataApprover.push(matrix)
    });

    dt_approver.clear().draw();
    dt_approver.rows.add(dataApprover).draw();
});

const getProfile= () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-list/profile",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].id
                    });
                }
                $('#profile').select2({
                    placeholder: "Select a Profile",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getInitiator= () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-list/profile",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].id
                    });
                }
                $('#initiator').select2({
                    placeholder: "Select an Initiator",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getEntity= () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-list/entity",
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
                $('#entity').select2({
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
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDistributor = (entityid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-data/distributor/entity-id",
            type        : "GET",
            data        : {PrincipalId: entityid},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].distributorId,
                        text: result.data[j].longDesc
                    });
                }
                $('#distributor').select2({
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
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getCategory= () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-list/category",
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
                $('#category').select2({
                    placeholder: "Select a Category",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSubActivityType = (categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-list/sub-activity-type",
            type        : "GET",
            dataType    : 'json',
            data        : {categoryId: categoryId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].Id,
                        text: result.data[j]['LongDesc']
                    });
                }
                $('#sub_activity_type').select2({
                    placeholder: "Select a Sub Activity Type",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-list/channel",
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
                $('#channel').select2({
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
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSubChannel = (channelid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-data/sub-channel/channel-id",
            type        : "GET",
            data        : {ChannelId: channelid},
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
                $('#sub_channel').select2({
                    placeholder: "Select a Sub Channel",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/matrix/promoapproval/data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;
                    if (values.header) {

                        $('#entity').val(values.header.entityId).trigger('change.select2');

                        await getDistributor(values.header.entityId);
                        $('#distributor').val(values.header.distributorId).trigger('change.select2');

                        $('#category').val(values.header.categoryId).trigger('change.select2');

                        await getSubActivityType(values.header.categoryId);
                        $('#sub_activity_type').val(values.header.subActivityTypeId).trigger('change.select2');

                        $('#channel').val(values.header.channelId).trigger('change.select2');

                        await getSubChannel(values.header.channelId);
                        $('#sub_channel').val(values.header.subChannelId).trigger('change.select2');

                        $('#initiator').val(values.header.initiator).trigger('change.select2');
                        $('#min_investment').val(formatMoney(values.header.minInvestment));
                        $('#max_investment').val(formatMoney(values.header.maxInvestment));

                    }
                    dt_approver.rows.add(values.detailMatrix).draw();
                    return resolve();
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        return resolve();
                    });
                }
            },
            complete:function(){
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}
