'use strict';

var swalTitle = "Reference Distributor";
var targetReference = document.querySelector(".card_reference");
var blockUIReference = new KTBlockUI(targetReference, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Uploading...</div>',
});
var access = true;

$(document).ready(async function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    blockUIReference.block();
    checkAccess();
    await getLisDistributor();
    $('#filter_distributor').select2().trigger('change');
    blockUIReference.release();
});

$('#filter_distributor').on('change', async function () {
    let idDistributor = $(this).val();
    let input_file = $('.input_file');
    for (let i=0; i<input_file.length; i++) {
        let row = $(input_file[i]).attr('data-row-file');
        await checkFile(idDistributor, row);
    }
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    let distributorId = $('#filter_distributor').val();
    let nameFile = $('#review_file_label_'+row).text();
    $.ajax({
        url         : "/references/distributor/check-file",
        type        : "GET",
        data        : {row: row, distributorId: distributorId},
        dataType    : 'json',
        async       : true,
        success: function(result) {
            if (!result.error) {
                let url = "/" + result.files[0];
                fetch(url)
                    .then(resp => resp.blob())
                    .then(blob => {
                        const url = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.style.display = 'none';
                        a.href = url;
                        a.download = nameFile;
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url);
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

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(errorThrown);
        }
    });
});

$('.input_file').change(function(e) {
    let row = $(this).attr('data-row-file');
    let elLabel = $('#review_file_label_' + row);
    let elInputFile = $('#file_' + row);
    let oldNameFile = elLabel.text();
    let fileName = (e.target.files.length > 0) ? e.target.files[0].name : 'Choose File';
    elLabel.text(fileName).attr('title', fileName);

    if (elInputFile[0].files[0].size > 10000000) {
        swal.fire({
            title: swalTitle,
            icon: "warning",
            text: 'Maximum file size 10Mb',
            showConfirmButton: true,
            confirmButtonText: 'OK',
            allowOutsideClick: false,
            allowEscapeKey: false,
        });
        elLabel.text(oldNameFile);
    } else if (checkNameFile(elInputFile.val())) {
        swal.fire({
            title: swalTitle,
            icon: "warning",
            text: "File name has special characters ~`!@#$%^&*+=()[]\\\';,/{}|\":<>? \n. These are not allowed\n",
            showConfirmButton: true,
            confirmButtonText: 'OK',
            allowOutsideClick: false,
            allowEscapeKey: false,
        });
        elLabel.text(oldNameFile);
    } else {
        uploadFile(elInputFile[0].files[0], row);
    }
});

const checkFile = (distributorId, row) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/references/distributor/check-file",
            type: "GET",
            data: {row: row, distributorId: distributorId},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    let fileName =  result.files[0].replace('assets/media/references/803/' + distributorId + "/" + row + "/", '');
                    $('#review_file_label_' + row).text(fileName).attr('title', fileName);
                } else {
                    let fileName = "";
                    if (access) {
                        fileName =  "Choose File";
                    }
                    $('#review_file_label_' + row).text(fileName).attr('title', fileName);
                }
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const uploadFile = (file, row) => {
    let distributorId = $('#filter_distributor').val();
    let formData = new FormData();
    formData.append('file', file);
    formData.append('row', row);
    formData.append('distributorId', distributorId);
    let url = '/references/distributor/upload';
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
            blockUIReference.block();
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
            blockUIReference.release();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message)
            Swal.fire({
                title: swalTitle,
                text: "Failed to upload file, an error occurred in the process",
                icon: "error",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
}

const checkAccess = () => {
    $.ajax({
        url         : "/check-form-access",
        type        : "GET",
        data        : {menuid: menu_id, access_name: 'update_rec'},
        dataType    : 'json',
        async       : true,
        success: function(result) {
            if (result.error) {
                access = false;
                Swal.fire({
                    title: swalTitle,
                    text: "You have no right to Edit Data",
                    icon: "warning",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    $('.input_file').remove();
                    $('.input-group-text').addClass('d-none');
                    $('.review_file_label').addClass('cursor-default form-control-solid-bg').css('border-radius', '.375rem').text("");
                    $('#filter_distributor').addClass('form-select-solid').prop('disabled', true);
                });
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

const checkNameFile = (value) => {
    let fullPath = value;
    let filename;
    if (fullPath) {
        let startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
        filename = fullPath.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
    }
    let format = /[&~!@#$%^&*()+\=\[\]{};':"\\|,<>\/?]/;
    return format.test(filename);
}

const getLisDistributor = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/references/distributor/list/distributor",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (result.data.distributorlist) {
                    let distributorList = result.data.distributorlist;
                    let data = [];
                    for (let j = 0, len = distributorList.length; j < len; ++j){
                        data.push({
                            id: distributorList[j].distributorId,
                            text: distributorList[j].distributorLongDesc
                        });
                    }
                    $('#filter_distributor').select2({
                        placeholder: "Select a Distributor",
                        width: '100%',
                        data: data
                    });
                }
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
