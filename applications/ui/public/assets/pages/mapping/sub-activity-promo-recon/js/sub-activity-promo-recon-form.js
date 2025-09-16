'use strict';

var validator, method, id;
var swalTitle = "Mapping Sub Activity Promo Recon";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    id = url_str.searchParams.get("id");

    if (method !== "update") {
        validator =  FormValidation.formValidation(document.getElementById('form_mapping_sub_activity_promo_recon'), {
            fields: {
                categoryId: {
                    validators: {
                        notEmpty: {
                            message: "Category must be enter"
                        },
                    }
                },
                subCategoryId: {
                    validators: {
                        notEmpty: {
                            message: "Sub Category must be enter"
                        },
                    }
                },
                activityId: {
                    validators: {
                        notEmpty: {
                            message: "Activity must be enter"
                        },
                    }
                },
                subActivityId: {
                    validators: {
                        notEmpty: {
                            message: "Sub Activity must be enter"
                        },
                    }
                },
                allowEdit: {
                    validators: {
                        notEmpty: {
                            message: "Action must be enter"
                        },
                    }
                },
            },
            plugins: {
                trigger: new FormValidation.plugins.Trigger,
                bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
            }
        });
    } else {
        validator =  FormValidation.formValidation(document.getElementById('form_mapping_sub_activity_promo_recon'), {
            fields: {
                allowEdit: {
                    validators: {
                        notEmpty: {
                            message: "Action must be enter"
                        },
                    }
                },
            },
            plugins: {
                trigger: new FormValidation.plugins.Trigger,
                bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
            }
        });
    }

    blockUI.block();
    disableButtonSave();
    if (method === "update") {
        $('#elCategoryId').html('<input type="text" class="form-control form-control-sm" id="categoryId" name="categoryId" readonly/>');
        $('#elSubCategoryId').html('<input type="text" class="form-control form-control-sm" id="subCategoryId" name="subCategoryId" readonly/>');
        $('#elActivityId').html('<input type="text" class="form-control form-control-sm" id="activityId" name="activityId" readonly/>');
        $('#elSubActivityId').html('<input type="text" class="form-control form-control-sm" id="subActivityId" name="subActivityId" readonly/>');
        $('#txt_info_method').text('Edit');
        getData(id).then(function () {
            blockUI.release();
            enableButtonSave();
        });
    } else {
        getListCategory().then(function () {
            blockUI.release();
            enableButtonSave();
        });
    }
});

$('#categoryId').on('change', async function () {
    blockUI.block();
    let elSubCategory = $('#subCategoryId');
    elSubCategory.empty();
    if ($(this).val() !== "") await getListSubCategory($(this).val());
    blockUI.release();
    elSubCategory.val('').trigger('change');
    validator.resetForm(true);
});

$('#subCategoryId').on('change', async function () {
    blockUI.block();
    let elActivity = $('#activityId');
    elActivity.empty();
    if ($(this).val() !== "") await getListActivity($(this).val());
    blockUI.release();
    elActivity.val('').trigger('change');
    validator.resetForm(true);
});

$('#activityId').on('change', async function () {
    blockUI.block();
    let elSubActivity = $('#subActivityId');
    elSubActivity.empty();
    if ($(this).val() !== "") await getListSubActivity($(this).val());
    blockUI.release();
    elSubActivity.val('').trigger('change');
    validator.resetForm(true);
});

$('#subActivityId').on('change', async function () {
    validator.resetForm(true);
});

$('#btn_save').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save");

            let formData = new FormData($('#form_mapping_sub_activity_promo_recon')[0]);

            let url = '/mapping/sub-activity-promo-recon/save';
            if (method === "update") {
                formData.append('subActivityId', id);
                url = '/mapping/sub-activity-promo-recon/update';
            }
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            blockUI.block();
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
                success: function(result, status, xhr, $form) {
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
                            window.location.href = '/mapping/sub-activity-promo-recon';
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
                complete: function() {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    blockUI.release();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.message)
                    Swal.fire({
                        title: swalTitle,
                        text: "Failed to save data, an error occurred in the process",
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        }
    });
});

const getListCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/sub-activity-promo-recon/list/category",
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
                $('#categoryId').select2({
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
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListSubCategory = (categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/sub-activity-promo-recon/list/sub-category/category-id",
            type        : "GET",
            dataType    : 'json',
            data        : {categoryId: categoryId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#subCategoryId').select2({
                    placeholder: "Select a Sub Category",
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

const getListActivity = (subCategoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/sub-activity-promo-recon/list/activity/sub-category-id",
            type        : "GET",
            dataType    : 'json',
            data        : {subCategoryId: subCategoryId},
            async       : true,
            success: function(result) {
                console.log(result);
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#activityId').select2({
                    placeholder: "Select an Activity",
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

const getListSubActivity = (activityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/sub-activity-promo-recon/list/sub-activity/activity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {activityId: activityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#subActivityId').select2({
                    placeholder: "Select a Sub Activity",
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

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/mapping/sub-activity-promo-recon/get-data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#categoryId').val(values.category);
                    $('#subCategoryId').val(values.subCategory);
                    $('#activityId').val(values.activity);
                    $('#subActivityId').val(values.subActivity);

                    $('#allowEdit').val(values.allowEdit).trigger('change.select2');

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
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}
