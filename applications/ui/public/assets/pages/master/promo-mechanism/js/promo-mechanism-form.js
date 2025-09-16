'use strict';

var validator, method, mechanismId, entity, activity, subActivity, sku, skuId;
var swalTitle = "Mechanism";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    $('#start_date').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "d-m-Y",
        disableMobile: "true",
        maxDate: $('#end_date').val()
    });

    $('#end_date').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "d-m-Y",
        disableMobile: "true",
        minDate: $('#start_date').val()
    });

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    mechanismId = url_str.searchParams.get("mechanismid");
    blockUI.block();
    disableButtonSave();
    if (method === 'update') {
        Promise.all([ getEntity(), getChannel(), getActivity() ]).then(async () => {
            await getData(mechanismId);
            $('#activity').val(activity).trigger('change');
            $('#txt_info_method').text('Edit Promo Mechanism ');
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ getEntity(), getActivity(), getChannel() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }

    const v_period = function () {
        return {
            validate: function () {
                if ($('#start_date').val() === "") {
                    return {
                        valid: false,
                        message: "Please enter a valid date"
                    };
                }
                if ($('#end_date').val() === "") {
                    return {
                        valid: false,
                        message: "Please enter a valid date"
                    };
                }
                return {
                    valid: true,
                };
            }
        }
    };
    const form = document.getElementById('form_mechanism');

    FormValidation.validators.v_period = v_period;

    validator = FormValidation.formValidation(form, {
        fields: {
            entity: {
                validators: {
                    notEmpty: {
                        message: "Entity must be enter"
                    },
                }
            },
            subcategory: {
                validators: {
                    notEmpty: {
                        message: "Sub Category must be enter"
                    },
                }
            },
            activity: {
                validators: {
                    notEmpty: {
                        message: "Activity must be enter"
                    },
                }
            },
            subactivity: {
                validators: {
                    notEmpty: {
                        message: "Sub Activity must be enter"
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
            sku: {
                validators: {
                    notEmpty: {
                        message: "Sku must be enter"
                    },
                }
            },
            start_date: {
                validators: {
                    v_period: {

                    }
                }
            },
            mechanism: {
                validators: {
                    notEmpty: {
                        message: "Mechanism Type must be enter"
                    },
                    stringLength: {
                        max: 255,
                        message: 'Mechanism Type must be less than 255 characters',
                    }
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })
});

$('#entity').on('change', async function () {
    let val = $(this).val();
    if(!method){
        blockUI.block();
    }
    if (val === "") {
        $('#sku').empty();
    } else {
        $('#sku').empty();
        await getSku(val);
        $('#sku').val(skuId).trigger('change');
        if(!method){
            $('#sku').val('').trigger('change');
        }
    }
    blockUI.release();
});

$('#activity').on('change', async function () {
    let val = $(this).val();
    if(!method){
        blockUI.block();
    }
    if (val === "") {
        $('#subactivity').empty();
    } else {
        $('#subactivity').empty();
        await getSubActivity(val);
        $('#subactivity').val(subActivity).trigger('change');
        if(!method){
            $('#subactivity').val('').trigger('change');
        }
    }
    blockUI.release();
});

$('#btn_back').on('click', function() {
    window.location.href = '/master/promo-mechanism';
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
    let formData = new FormData($('#form_mechanism')[0]);
    formData.append('subActivityId', $('#subactivity').val());
    formData.append('entityText', $('#entity :selected').text());
    formData.append('subCategoryText', $('#subcategory :selected').text());
    formData.append('activityText', $('#activity :selected').text());
    formData.append('subActivityText', $('#subactivity :selected').text());
    formData.append('skuText', $('#sku :selected').text());
    formData.append('channelText', $('#channel :selected').text());

    let url = '/master/promo-mechanism/save';
    if (method === "update") {
        formData.append('id', mechanismId);
        url = '/master/promo-mechanism/update';
    }
    $.get('/refresh-csrf').done(function(data) {
        $('meta[name="csrf-token"]').attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
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
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
            },
            success: function(result, status, xhr, $form) {
                if (!result.error) {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "success",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        if (exit) {
                            window.location.href = '/master/promo-mechanism';
                        } else {
                            let form = document.querySelectorAll("#form_mechanism input[type=text]");
                            formReset(form);
                        }
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete: function() {
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
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    });
}

$('#start_date').on('change', function () {
    let el_start = $('#start_date');
    let el_end = $('#end_date');
    if (el_start.val() === "") el_start.val('');
    el_end.flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "d-m-Y",
        disableMobile: "true",
        minDate: el_start.val()
    });
});

$('#end_date').on('change', function () {
    let el_start = $('#start_date');
    let el_end = $('#end_date');
    el_start.flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "d-m-Y",
        disableMobile: "true",
        maxDate: el_end.val()
    });
});

const getEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/promo-mechanism/get-list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].longDesc,
                        text: result.data[j].longDesc
                    });
                }
                $('#entity').select2({
                    placeholder: "Select a Entity",
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

const getActivity = (activity) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/promo-mechanism/get-data/attribute",
            type        : "GET",
            data        : {attribute: "activity", longDesc: activity},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].longDesc,
                        text: result.data[j].longDesc
                    });
                }
                $('#activity').select2({
                    placeholder: "Select a Entity",
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

const getSubActivity = (activityLongDesc) => {
    var data = [];
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/promo-mechanism/get-data/attribute",
            type        : "GET",
            data        : {attribute: "subactivity", longDesc: activityLongDesc},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].longDesc,
                        text: result.data[j].longDesc
                    });
                }
                $('#subactivity').select2({
                    placeholder: "Select a Sub activity",
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

const getChannel= () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/promo-mechanism/get-list/channel",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                data.unshift({ id: 0, text: "ALL" });
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
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSku = (entityLongDesc) => {
    var data = [];
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/promo-mechanism/get-data/attribute",
            type        : "GET",
            data        : {attribute: "product", longDesc: entityLongDesc},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#sku').select2({
                    placeholder: "Select a Sku",
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
            url: "/master/promo-mechanism/data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    entity = values[0].entity;
                    activity = values[0].activity;
                    subActivity = values[0].subActivity;
                    skuId = values[0].productId;
                    sku = values[0].product;
                    $('#entity').val(values[0].entity).trigger('change');
                    $('#activity').val(values[0].activityDesc).trigger('change');
                    $('#channel').val(values[0].channelId).trigger('change');
                    $('#start_date').val(formatDateOptima(values[0].startDate));
                    $('#end_date').val(formatDateOptima(values[0].endDate));
                    $('#mechanism').val(values[0].mechanism);
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
    for (var i = 0, len = elements.length; i < len; ++i) {
        elements[i].value = "";
    }
}
