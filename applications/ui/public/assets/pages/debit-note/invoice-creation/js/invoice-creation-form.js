'use strict';

let swalTitle = "Invoice Creation";
let id, method, validator, dialerObject, validatorUpload, sumAmount = 0, dt_dn_detail, listDistributor = [], listTaxLevel = [], listEntity = [], listCategory = [], arrChecked = [];
let entityId = "", distributorId, taxLevelId = "", categoryId = "";
let elDtDnDetail = $('#dt_dn_detail');

(function(window, document, $) {
    $('#form-upload').hide();
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

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        digits: 0,
        groupSeparator: ",",
    }).mask("#ppn");

    if (method === 'update') {
        blockUI.block();
        $('#entityId').remove();
        $('#taxLevel').remove();
        $('#categoryId').remove();

        $('#dialer_period').html('<input type="text" class="form-control form-control-sm form-control-solid-bg" name="dnPeriod" id="dnPeriod" autocomplete="off" readonly/>');
        $('#dynamicElEntity').html('<input class="form-control form-control-sm form-control-solid-bg" type="text" name="entityId" id="entityId" autocomplete="off" readonly/>');
        $('#dynamicElTaxLevel').html('<input class="form-control form-control-sm form-control-solid-bg" type="text" name="taxLevel" id="taxLevel" autocomplete="off" readonly/>');
        $('#dynamicElCategory').html('<input class="form-control form-control-sm form-control-solid-bg" type="text" name="categoryId" id="categoryId" autocomplete="off" readonly/>');

        Promise.all([getData(id)]).then(async () => {
            blockUI.release();
        });
    } else {
        Promise.all([getListEntity(), getDataDistributor(), getListCategory(), getListTaxLevel()]).then(async () => {
        });
    }

    validator = FormValidation.formValidation(document.getElementById('form_invoice'), {
        fields: {
            dnPeriod: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            invoiceDesc: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            entityId: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            taxLevel: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            categoryId: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            ppn: {
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
    });

    validatorUpload = FormValidation.formValidation(document.querySelector("#form_upload_validate"), {
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

    dt_dn_detail = elDtDnDetail.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[2, 'asc']],
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        scrollY: "42vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 10,
                searchable: false,
                orderable: false,
                className: 'text-nowrap dt-body-start text-center align-middle',
                checkboxes: {
                    'selectRow': false,
                },
                createdCell:  function (td, cellData, rowData){
                    if (rowData.hasOwnProperty('checkFlag')) {
                        if (rowData.checkFlag) {
                            this.api().cell(td).checkboxes.select();
                        } else {
                            this.api().cell(td).checkboxes.deselect();
                        }
                    }
                },
            },
            {
                targets: 1,
                data: 'id',
                width: 10,
                searchable: false,
                orderable: false,
                className: 'text-nowrap dt-body-start text-center align-middle',
                render: function (data) {
                    if(method === 'update'){
                        return '\
                            <div class="dropdown">\
                                <a href="/dn/invoice/reject?method=update' + '&idn=' + id + '&id=' + data + '" class="btn btn-optima btn-sm btn-clean" title="Reject" ">\
                                    <i class="la la-mail-reply"></i>\
                                </a>\
                            </div>\
                        ';
                    } else {
                        return '\
                            <div class="dropdown">\
                                <a href="/dn/invoice/reject?method=add' + '&id=' + data + '" class="btn btn-optima btn-sm btn-clean" title="Reject" ">\
                                    <i class="la la-mail-reply"></i>\
                                </a>\
                            </div>\
                        ';
                    }
                },
            },
            {
                targets: 2,
                title: 'Dn Number',
                data: 'refId',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'DN Description',
                data: 'activityDesc',
                width: 500,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'DPP',
                data: 'dpp',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,0);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 6,
                title: 'PPN',
                data: 'ppnAmt',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,0);
                    } else {
                        return 0;
                    }
                }
            },
            {
                targets: 7,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,0);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 8,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'Tax Level',
                data: 'taxLevel',
                width: 100,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function () {
            $('#dt_dn_detail_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_dn_detail.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                sumAmount = 0;
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        // if (data[i]['checkFlag']) {
                            arrChecked.push({
                                dnid: data[i].id,
                            });
                        // }
                        sumAmount += data[i]['dpp']
                    }
                }


                let ppn = $('#ppn').val();
                let ppnAmount = (sumAmount * ppn) / 100;
                let invoiceAmount = sumAmount + ppnAmount;
                $('#dppAmount').val(formatMoney(sumAmount, 0));
                $('#invoiceAmount').val(formatMoney(invoiceAmount, 0));

            });
        },
        drawCallback: function (settings, json) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        let strMessage = e.jqXHR['responseJSON'].message
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

    elDtDnDetail.on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_dn_detail.row(this.closest('tr')).data();

        // push into array
        if (this.checked) {
            arrChecked.push({
                dnid: rows.id,
            });
        } else {
            let index = arrChecked.findIndex(p => p.dnid === rows.id);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }

        // calculate invoice amount
        let elDppAmount = $('#dppAmount');
        let dpp = rows.dpp;
        if (method === "update") sumAmount = parseFloat(elDppAmount.val().toString().replace(/,/g, ''));
        if (this.checked) {
            sumAmount += parseFloat(dpp);
        } else {
            sumAmount -= parseFloat(dpp);
        }
        let ppn = $('#ppn').val();

        let ppnAmount = sumAmount * (ppn / 100);
        let invoiceAmount = sumAmount + ppnAmount;

        elDppAmount.val(formatMoney(sumAmount, 0));
        $('#invoiceAmount').val(formatMoney(invoiceAmount, 0));
    });

});

$('#dt_dn_detail_search').on('keyup', function() {
    dt_dn_detail.search(this.value).draw();
});

$('#dnPeriod').on('change', async function () {
    if (method !== "update") {
        removeTickRows();
        let dnPeriod = $(this).val();
        if (!$("#dnPeriod").is(":focus")) {
            blockUI.block();
            await getDataDN($('#entityId').val(), $('#taxLevel').val(), this.value, $('#categoryId').val());
            blockUI.release();
        }
        if (parseInt(dnPeriod) < 2024) {
            $('#rowCategory').addClass('d-none');
            $('#labelCategory').removeClass('required');
            $('#categoryId').val('').trigger('change');
            validator.disableValidator('categoryId', 'notEmpty');
        } else {
            $('#rowCategory').removeClass('d-none');
            $('#labelCategory').addClass('required');
            validator.enableValidator('categoryId', 'notEmpty');
        }
    }
});

$('#entityId').on('change', async function () {
    blockUI.block();
    removeTickRows();
    entityId = $(this).val();
    await getDataDN(this.value,$('#taxLevel').val(), $('#dnPeriod').val(), $('#categoryId').val());
    blockUI.release();
});

$('#categoryId').on('change', async function () {
    blockUI.block();
    removeTickRows();
    categoryId = $(this).val();
    await getDataDN($('#entityId').val(), $('#taxLevel').val(), $('#dnPeriod').val(), this.value);
    blockUI.release();
});

$('#taxLevel').on('change', async function () {
    blockUI.block();
    removeTickRows();
    taxLevelId = $(this).val();
    await getDataDN($('#entityId').val(), this.value, $('#dnPeriod').val(), $('#categoryId').val());
    blockUI.release();
});

$('#ppn').on('keyup', function () {
    calculation();
});

$('#btn_download').on('click', function() {
    let url = '/assets/media/templates/Template_DN_Upload_Validation.xlsx';
    fetch(url)
        .then(resp => resp.blob())
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            // the filename you want
            a.download = 'Template DN Invoice.xlsx';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        });
});

$('#btn-upload').on('click', function() {
    $('#form-upload').toggle();
});

$('#btn_upload').on('click', function() {
    let elDnPeriod = $('#dnPeriod');
    validatorUpload.validate().then(function (status) {
        if (status === "Valid") {
            let dnPeriod = elDnPeriod.val();
            if (parseInt(dnPeriod) < 2024) {
                if (entityId === "" && taxLevelId === "") {
                    return Swal.fire({
                        title: swalTitle,
                        text: 'Please select an Entity & Tax Level',
                        icon: "warning",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            } else {
                if (entityId === "" && taxLevelId === "" && categoryId === "") {
                    return Swal.fire({
                        title: swalTitle,
                        text: 'Please select an Entity, Category, & Tax Level',
                        icon: "warning",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            }
            if (entityId === "") {
                return Swal.fire({
                    title: swalTitle,
                    text: 'Please select an Entity',
                    icon: "warning",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
            if (parseInt(dnPeriod) > 2023 && categoryId === "") {
                return Swal.fire({
                    title: swalTitle,
                    text: 'Please select a Category',
                    icon: "warning",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
            if (taxLevelId === "") {
                return Swal.fire({
                    title: swalTitle,
                    text: 'Please select a Tax Level',
                    icon: "warning",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
            let e = document.querySelector("#btn_upload");
            let formData = new FormData($('#form_upload_validate')[0]);
            formData.append('dnPeriod', elDnPeriod.val());
            formData.append('entity', entityId);
            formData.append('taxLevel', taxLevelId);
            formData.append('categoryId', categoryId);
            if (method === "update") {
                formData.append('invoiceId', id);
            }

            let url = '/dn/invoice/upload-xls';
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
                success: function (result) {
                    if (!result.error) {
                        $('#dppAmount').val(0);
                        $('#invoiceAmount').val(0);
                        Swal.fire({
                            title: 'Files Uploaded!',
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            // clear checked cache
                            let el_header = $('#dt_dn_detail_wrapper #dt-checkbox-header');
                            if (el_header[0].checked) {
                                el_header.trigger('click');
                            } else {
                                el_header.trigger('click');
                                el_header.trigger('click');
                            }

                            // clear data on datatable
                            dt_dn_detail.clear().draw();

                            // add data on datatable
                            dt_dn_detail.rows.add(result.data).draw();

                            sumAmount = 0;
                            for (let i=0; i<result.data.length; i++) {
                                if (result.data[i]['checkFlag']) {
                                    arrChecked.push({
                                        dnid: result.data[i]['id']
                                    });

                                    sumAmount += result.data[i]['dpp'];
                                }
                            }

                            $('#dppAmount').val(formatMoney(sumAmount,0));
                            calculation();
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "OK",
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

$('#btn_save').on('click', function () {
    validator.validate().then(async function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save");
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            let url;
            let res_method;
            let message;
            let error = true;

            console.log(arrChecked);

            if (method !== 'update') {
                if (arrChecked.length > 0) {
                    url = '/dn/invoice/save';
                    message = 'Save successfully';
                    res_method = await submit(arrChecked, url);
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
            } else {
                url = '/dn/invoice/update';
                message = 'Update successfully';
                res_method = await submit(arrChecked, url);
            }

            if (!res_method) {
                error = false;
            }

            if (res_method) {
                Swal.fire({
                    title: swalTitle,
                    text: message,
                    icon: "success",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    window.location.href = '/dn/invoice';
                });
            }

            if (!error) {
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
                Swal.fire({
                    title: swalTitle,
                    text: "Save Failed",
                    icon: "error",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: { confirmButton: "btn btn-optima" }
                });
            }
        } else {
            Swal.fire({
                title: "Data not valid",
                icon: "warning",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

const submit = (dataRowsChecked, url) =>  {
    return new Promise(function (resolve) {
        let formData = new FormData($('#form_invoice')[0]);
        if (method === 'update') {
            formData.append('id', id);
            formData.append('entityId', entityId);
            formData.append('categoryId', categoryId);
            formData.append('taxLevel', taxLevelId);
        }
        formData.append('dppAmount', parseFloat($('#dppAmount').val().replace(/,/g, '')) );
        formData.append('invoiceAmount', parseFloat($('#invoiceAmount').val().replace(/,/g, '')));
        formData.append('dnid', JSON.stringify(dataRowsChecked));

        $.ajax({
            type        : 'POST',
            url         : url,
            data        : formData,
            dataType    : "JSON",
            async       : true,
            cache       : false,
            contentType : false,
            processData : false,
            success: function (result) {
                if (!result.error) {
                    return resolve(true);
                } else {
                    return resolve(false);
                }
            },
            complete: function() {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown)
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR['responseJSON'].message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-primary" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                return resolve(false);
            }
        });
    });
}

const getData = (id) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/invoice/get-data/id",
            type: "GET",
            data: {id: id},
            dataType: 'json',
            async: true,
            success: async function (result) {
                if (!result.error) {
                    let value = result.data;
                    $('#txt_info_method').text('Edit ' + value['refId']);
                    $('#invoiceDesc').val(value['invoiceDesc']);
                    $('#ppn').val(formatMoney(value['ppn'],0));

                    $('#invoiceAmount').val(formatMoney(value['invoiceAmount'],0));
                    $('#dppAmount').val(formatMoney(value['dpp'],0));
                    sumAmount = value['dpp'];

                    entityId = value['entityId'];
                    distributorId = value['distributorId'];
                    categoryId = value['categoryId'];
                    taxLevelId = value['taxLevel'];

                    $('#dnPeriod').val(value['dnPeriod']);
                    $('#entityId').val(value['entityDesc']);
                    $('#distributorId').val(value['distributorDesc']);
                    $('#categoryId').val(value['categoryDesc']);
                    $('#taxLevel').val(value['taxLevelDesc']);

                    if (parseInt(value['dnPeriod']) < 2024 ) {
                        $('#labelCategory').removeClass('required');
                        validator.disableValidator('categoryId', 'notEmpty');
                        $('#rowCategory').addClass('d-none');
                    }

                    let detailDn = value['standartDetailDN'];
                    for (let i=0; i<detailDn.length; i++) {
                        detailDn[i]['checkFlag'] = true;
                        arrChecked.push({
                            dnid: detailDn[i]['id']
                        });
                    }

                    dt_dn_detail.rows.add(detailDn).draw();
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR['responseJSON'].message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-optima" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                return reject(jqXHR.responseText);
            },
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataDN = (entityId, taxLevelId, dnPeriod, categoryId) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/invoice/get-data/dn",
            type: "GET",
            data: {
                entityId: entityId,
                TaxLevel: taxLevelId,
                dnPeriod: dnPeriod,
                categoryId: categoryId
            },
            dataType: 'json',
            async: true,
            success: async function (result) {
                dt_dn_detail.clear().draw();
                dt_dn_detail.rows.add(result.data).draw();
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR['responseJSON'].message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-optima" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                return reject(jqXHR.responseText);
            },
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/invoice/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].shortDesc + ' - ' +result.data[j].longDesc,
                        longDesc: result.data[j].longDesc,
                    });
                }
                listEntity = data;
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
                console.log(errorThrown);
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
            url         : "/dn/invoice/list/category",
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
                listCategory = data;
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
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataDistributor = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/invoice/get-data/distributor",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                distributorId = result.data[0].distributorId;
                $('#distributorId').val(result.data[0].companyName)
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

const getListTaxLevel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/invoice/list/tax-level",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].materialNumber,
                        text: result.data[j].materialNumber + ' - ' +result.data[j].description,
                    });
                }
                listTaxLevel = data;
                $('#taxLevel').select2({
                    placeholder: "Select a Tax Level",
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

const calculation = () => {
    let dppAmount = parseFloat(document.getElementById("dppAmount").value.replace(/,/g, ''));
    let ppn = parseFloat(document.getElementById("ppn").value.replace(/,/g, ''))

    let ppnAmount = dppAmount * (ppn / 100);
    let invoiceAmount = dppAmount + ppnAmount;

    $("#invoiceAmount").val(formatMoney(invoiceAmount,0));
}

const removeTickRows = () => {
    let el_header_detail = $('#dt-checkbox-header');
    if (el_header_detail.prop("checked") ) {
        $(el_header_detail).trigger('click');
    }
    if (el_header_detail.prop("indeterminate")) {
        $(el_header_detail).trigger('click');
        $(el_header_detail).trigger('click');
    }
}
