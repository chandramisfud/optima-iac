'use strict';

let dt_upload_history, validator;
let swalTitle = "Transfer to SAP - Payment";

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    disableButtonSave();
    Promise.all([ getEntity() ]).then(async () => {
        enableButtonSave();
    });

    validator = FormValidation.formValidation(document.querySelector("#form_sap_payment"), {
        fields: {
            file: {
                selector: '[data-stripe="file"]',
                validators: {
                    notEmpty: {
                        message: 'Choose file..',
                    },
                },
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    dt_upload_history = $('#dt_upload_history').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-4'i><'col-sm-7'p>>",
        order: [[0, 'asc']],
        ajax: {
            url: '/tools/sap-payment/get-list/upload-history',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "50vh",
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Upload On',
                data: 'uploadOn',
                className: 'text-nowrap align-middle',
                width: 250,
                render: function (data) {
                    return formatDateTime(data);
                }
            },
            {
                targets: 1,
                title: 'Upload By',
                data: 'uploadBy',
                className: 'text-nowrap align-middle',
                width: 250,
            },
            {
                targets: 2,
                title: 'Filename',
                data: 'fileName',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strMessage = message
        if(strMessage==="") strMessage = "Please contact your vendor"
        Swal.fire({
            text: strMessage,
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

$('#filter_collapsible').on('hidden.bs.collapse', function () {
    $('div.dataTables_scrollBody').height( "63vh" );
});

$('#btn_export_excel').on('click', function() {
    window.open("/master/promo-mechanism/export-xls", "_blank");
});

$('#filter_entity').on('change', async function () {
    let val = $(this).val();
    blockUI.block();
    if (val === "") {
        $('#filter_distributor').empty();
    } else {
        $('#filter_distributor').empty();
        await getDistributor(val);
    }
    blockUI.release();
});

$('#btn_generate_xml_batch_name').on('click', function() {
    let e = document.querySelector("#btn_generate_xml_batch_name");
    let entity = $('#filter_entity').val();

    if(entity=="" || entity==null) { entity = "0"; }
    let distributor = $("#filter_distributor").select2().val();
    let distributor_list = [];
    if(distributor.length===0 || distributor[0]===""){
        distributor_list.push(0)
    }else{
        for (var i = 0; i < distributor.length; i++) {
            distributor_list.push(distributor[i])
        }
    }
    let dataEntity = $('#filter_entity').select2('data')

    let EntityName = dataEntity[0].text
    if(EntityName==='') EntityName='All'

    let url = '/tools/sap-payment/xml-generate/batch-name?entity='  + entity + "&entityname=" + EntityName + "&distributor=" + encodeURIComponent(JSON.stringify(distributor_list))
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_generate_xml').on('click', async function() {
    let elEntity = $('#filter_entity');

    // data filter
    let entity = (elEntity.val() === "" || elEntity.val() === null) ? "0" : elEntity.val();
    let distributor = $("#filter_distributor").select2().val();
    let distributor_list = [];
    if (distributor.length === 0 || distributor[0] === ""){
        distributor_list.push(0);
    } else {
        for (let i=0; i<distributor.length; i++) {
            distributor_list.push(distributor[i]);
        }
    }

    let uuid = generateUUID(6);

    blockUI.block();

    await getDataDistributorPaymentAloneNonBatching(uuid, entity, distributor_list);
    await getDataDistributorPaymentAloneBatching(uuid, entity, distributor_list);
    await getDataXmlNonBatching(uuid, entity, distributor_list);
    await getDataXmlBatching(uuid, entity, distributor_list);
    await flaggingPayment(uuid, entity, distributor_list);

    downloadURL(uuid, entity, distributor_list, '/tools/sap-payment/xml-generate/download/payment-alone-non-batching');
    downloadURL(uuid, entity, distributor_list, '/tools/sap-payment/xml-generate/download/payment-alone-batching');
    downloadURL(uuid, entity, distributor_list, '/tools/sap-payment/xml-generate/download/xml-non-batching');
    downloadURL(uuid, entity, distributor_list, '/tools/sap-payment/xml-generate/download/xml-batching');

    blockUI.release();
});

$('.field_upload').on('change', function() {
    let file_size = $(this)[0].files[0].size;
    if (file_size > 10000000) {
        Swal.fire({
            title: "Mechanism",
            text: "Ukuran file lebih dari 10Mb, mohon pilih file yang ukurannya kurang dari 10Mb",
            icon: "warning",
            confirmButtonText: "Ok",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: { confirmButton: "btn btn-optima" }
        }).then(function () {
            $('.field_upload').val(null);
            validator.resetForm(true);
        });
    }
});

$('#btn_upload').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            var formData = new FormData($('#form_sap_payment')[0]);
            let url = '/tools/sap-payment/upload-xml';
            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                // enctype: "multipart/form-data",
                beforeSend: function () {
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                },
                success: function (result, status, xhr, $form) {
                    if (!result.error) {
                        Swal.fire({
                            title: 'Files Uploaded!',
                            icon: "success",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            dt_upload_history.ajax.reload();
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            dt_upload_history.ajax.reload();
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
                        text: "Files Upload Failed",
                        icon: "error",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        } else {
            Swal.fire({
                title: swalTitle,
                text: 'Choose file...',
                icon: "warning",
                confirmButtonText: "Ok",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

const getEntity= () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/tools/sap-payment/get-list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].shortDesc + '- ' +result.data[j].longDesc
                    });
                }
                $('#filter_entity').select2({
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
        return reject(e);
    });
}

const getDistributor = (entityid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/tools/sap-payment/get-data/distributor/entity-id",
            type        : "GET",
            data        : {PrincipalId:entityid},
            dataType    : 'json',
            async       : true,
            success: function (result) {
                let data = [];
                for (var j = 0, len = result.data.length; j < len; ++j) {
                    data.push({
                        id: result.data[j].distributorId,
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
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataDistributorPaymentAloneNonBatching = async (uuid, entity, distributor) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/tools/sap-payment/xml-generate/data/payment-alone-non-batching",
            type: "GET",
            data: {uuid: uuid, entity: entity, distributor: JSON.stringify(distributor)},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    return resolve(1);
                } else {
                    return resolve(0);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                reject(errorThrown);
                console.log(jqXHR.responseText);
                return resolve(0);
            },
        });
    });
}

const getDataDistributorPaymentAloneBatching = async (uuid, entity, distributor) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/tools/sap-payment/xml-generate/data/payment-alone-batching",
            type: "GET",
            data: {uuid: uuid, entity: entity, distributor: JSON.stringify(distributor)},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    return resolve(1);
                } else {
                    return resolve(0);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                reject(errorThrown);
                console.log(jqXHR.responseText);
                return resolve(0);
            },
        });
    });
}

const getDataXmlNonBatching = async (uuid, entity, distributor) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/tools/sap-payment/xml-generate/data/xml-non-batching",
            type: "GET",
            data: {uuid: uuid, entity: entity, distributor: JSON.stringify(distributor)},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    return resolve(1);
                } else {
                    return resolve(0);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                reject(errorThrown);
                console.log(jqXHR.responseText);
                return resolve(0);
            },
        });
    });
}

const getDataXmlBatching = async (uuid, entity, distributor) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/tools/sap-payment/xml-generate/data/xml-batching",
            type: "GET",
            data: {uuid: uuid, entity: entity, distributor: JSON.stringify(distributor)},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    return resolve(1);
                } else {
                    return resolve(0);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                reject(errorThrown);
                console.log(jqXHR.responseText);
                return resolve(0);
            },
        });
    });
}

const flaggingPayment = async (uuid, entity, distributor) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/tools/sap-payment/xml-generate/flagging-payment",
            type: "GET",
            data: {uuid: uuid, entity: entity, distributor: JSON.stringify(distributor)},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    return resolve(1);
                } else {
                    return resolve(0);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                reject(errorThrown);
                console.log(jqXHR.responseText);
                return resolve(0);
            },
        });
    });
}

const downloadURL = (uuid, entity, distributor_list, url) => {
    let hiddenIFrameID = 'hiddenDownloader' + uuid;
    let iframe = document.createElement('iframe');
    iframe.id = hiddenIFrameID;
    iframe.style.display = 'none';
    document.body.appendChild(iframe);
    iframe.src = url + "?uuid=" + uuid + "&entity=" + entity + "&distributor=" + distributor_list;
}
