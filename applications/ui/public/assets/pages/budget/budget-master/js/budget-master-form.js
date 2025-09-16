'use strict';

var validator, method, id, distributorId, isAllocated;
var swalTitle = "Budget Master";
var dialerObject;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        onfocus: "",
        digits: 0,
        groupSeparator: ",",
    }).mask("#budgetAmount");

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    id = url_str.searchParams.get("id");

    if (method === 'update') {
        blockUI.block();
        disableButtonSave();
        Promise.all([ getListEntity(), getListCategory() ]).then(async () => {
            await getData(id);
            await getListDistributor($('#entityId').val());
            $('#distributorId').val(distributorId).trigger('change');
            if (isAllocated) {
                Swal.fire({
                    title: swalTitle,
                    text: "This Data already Allocated",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
                $('#year').attr('disabled', true);
                $('#longDesc').attr('disabled', true);
                $('#budgetAmount').attr('disabled', true);
                $('#entityId').attr('disabled', true);
                $('#categoryId').attr('disabled', true);
                $('#distributorId').attr('disabled', true);
                $('#btn_save').addClass('d-none');
                $('#dialer_period button').addClass('d-none');
            }
            $('#txt_info_method').text('Edit');
            enableButtonSave();
            blockUI.release();
        });
    } else {
        dialerObject.setValue(new Date().getFullYear());
        blockUI.block();
        disableButtonSave();
        Promise.all([getListEntity(), getListCategory() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }

    validator =  FormValidation.formValidation(document.getElementById('form_budget_master'), {
        fields: {
            longDesc: {
                validators: {
                    notEmpty: {
                        message: "Budget Name must be enter"
                    },
                }
            },
            entityId: {
                validators: {
                    notEmpty: {
                        message: "Entity must be enter"
                    },
                }
            },
            distributorId: {
                validators: {
                    notEmpty: {
                        message: "Distributor must be enter"
                    },
                }
            },
            categoryId: {
                validators: {
                    notEmpty: {
                        message: "Category must be enter"
                    },
                }
            },
            year: {
                validators: {
                    notEmpty: {
                        message: "year must be enter"
                    },
                }
            },
            budgetAmount: {
                validators: {
                    notEmpty: {
                        message: "Budget Amount must be enter"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
        }
    });
});

$('#entityId').on('change', async function () {
    blockUI.block();
    $('#distributorId').empty();

    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#distributorId').val('').trigger('change');
});

$('#btn_save_exit').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save_action");
            save(true, e);
        }
    });
});

$('#btn_save_add').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save_action");
            save(false, e);
        }
    });
});

$('#btn_save').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save");
            save(true, e);
        }
    });
});

const save = (exit, e) => {
    let formData = new FormData($('#form_budget_master')[0]);
    let url = '/budget/master/save';
    if (method === "update") {
        url = '/budget/master/update';
        formData.append('id', id);
    }
    $.get('/refresh-csrf').done(function(data) {
        $('meta[name="csrf-token"]').attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });
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
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
            },
            success: function (result, status, xhr, $form) {
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
                        if (exit) {
                            window.location.href = '/budget/master';
                        } else {
                            let form = document.querySelectorAll("#form_budget_master input[type=text]");
                            formReset(form);
                        }
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
    });
}

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/master/list/entity",
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
                $('#entityId').select2({
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
            url         : "/budget/master/list/distributor/entity-id",
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
                $('#distributorId').select2({
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

const getListCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/master/list/category",
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

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/budget/master/get-data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#year').val(values.periode);
                    dialerObject.setValue(parseInt(values.periode));
                    $('#longDesc').val(values.budgetMasterLongDesc);
                    $('#budgetAmount').val(values.budgetAmount);
                    $('#entityId').val(values.principalId).trigger('change.select2');
                    $('#categoryId').val(values.categoryId).trigger('change');
                    distributorId = values.distributorId;
                    isAllocated = values.isAllocated;
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete:function(){
                return resolve();
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

const formReset = (elements) => {
    for (let i = 0, len = elements.length; i < len; ++i) {
        elements[i].value = "";
    }
}
