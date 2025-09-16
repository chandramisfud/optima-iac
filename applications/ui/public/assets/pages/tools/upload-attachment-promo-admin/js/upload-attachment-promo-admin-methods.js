'use strict';

var swalTitle = "Upload Promo Attachment";
var dt_attachment_promo, mydropzone;
var heightContainer = 670;
$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    mydropzone = new Dropzone("#dropzone_promo_admin", {
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        },
        type: 'POST',
        url: "/tools/upload-attachment-promo-admin/temp", // Set the url for your upload script location
        paramName: "file",
        maxFilesize: 10, // MB
        addRemoveLinks: true,
        init: function() {
            this.on("error", function(file, message) {
                this.removeFile(file);
            });
            this.on("addedfile", function(file) {
                setTimeout(function() {
                    if (file.size === 0 || file.size > 10000000) {
                        swal.fire({
                            title: 'File Size must not be 0 bytes and  should not exceed 10MB',
                            type: 'error',
                            // showCancelButton: true,
                            confirmButtonText: 'OK'
                        }).then(function (result) {
                            if (result.value) {
                                var fileName = file.name;
                                $.ajax({
                                    headers: {
                                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                                    },
                                    type: 'POST',
                                    url: '/tools/upload-attachment-promo-admin/temp-delete',
                                    data: {name: fileName, request: 'delete'},
                                    // data: {filename: name},
                                    success: function (data) {
                                        //console.log("File has been successfully removed!!");
                                    },
                                    error: function (e) {
                                        //console.log(e);
                                    }
                                });
                                let fileRef;
                                return (fileRef = file.previewElement) != null ? fileRef.parentNode.removeChild(file.previewElement) : void 0;
                            }
                        });
                    }
                }, 1000);

                // Get error
                this.on("uploadprogress", function(file, progress) {
                    //console.log("File progress", progress);
                });

                $('.dz-remove').on('click', function() {
                    var fileName = file.name;
                    $.ajax({
                        headers: {
                            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                        },
                        type: 'POST',
                        url: '/tools/upload-attachment-promo-admin/temp-delete',
                        data: {name: fileName,request: 'delete'},
                        success: function (data){
                            console.log("File has been successfully removed!!");
                        },
                        error: function(e) {
                            console.log(e);
                        }});
                    var fileRef;
                    return (fileRef = file.previewElement) != null ?
                        fileRef.parentNode.removeChild(file.previewElement) : void 0;
                });
            })
        },
        success: function(file, response)
        {

        },
        error: function(file, response)
        {
            return false;
        }
    });
    $('.dropzone').css('background-color', '#fff !important');

    dt_attachment_promo = $('#dt_attachment_promo').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        // order: [[1, 'asc']],
        // ajax: {
        //     url: url_datatable,
        //     type: 'get',
        // },
        ordering: false,
        processing: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                width: 250,
                title: 'Promo Number',
                data: 'PromoNumber',
                className: 'text-nowrap align-middle me-5',
            },
            {
                targets: 1,
                title: 'Attachment 1',
                data: 'attach1',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result1 == 'OK') {
                            return data + '&nbsp;<i class="fa fa-check" style="color:green" /*aria-hidden="true"*/></i>';
                        } else {
                            return data + '&nbsp;<i class="fa fa-times" style="color:red" /*aria-hidden="true"*/></i>';
                        }
                    }
                }
            },
            {
                targets: 2,
                title: 'Attachment 2',
                data: 'attach2',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result2 == 'OK') {
                            return data + '&nbsp;<i class="fa fa-check" style="color:green" /*aria-hidden="true"*/></i>';
                        } else {
                            return data + '&nbsp;<i class="fa fa-times" style="color:red" /*aria-hidden="true"*/></i>';
                        }
                    }
                }
            },
            {
                targets: 3,
                title: 'Attachment 3',
                data: 'attach3',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result3 == 'OK') {
                            return data + '&nbsp;<i class="fa fa-check" style="color:green" /*aria-hidden="true"*/></i>';
                        } else {
                            return data + '&nbsp;<i class="fa fa-times" style="color:red" /*aria-hidden="true"*/></i>';
                        }
                    }
                }
            },
            {
                targets: 4,
                title: 'Attachment 4',
                data: 'attach4',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result4 == 'OK') {
                            return data + '&nbsp;<i class="fa fa-check" style="color:green" /*aria-hidden="true"*/></i>';
                        } else {
                            return data + '&nbsp;<i class="fa fa-times" style="color:red" /*aria-hidden="true"*/></i>';
                        }
                    }
                }
            },
            {
                targets: 5,
                title: 'Attachment 5',
                data: 'attach5',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result5 == 'OK') {
                            return data + '&nbsp;<i class="fa fa-check" style="color:green" /*aria-hidden="true"*/></i>';
                        } else {
                            return data + '&nbsp;<i class="fa fa-times" style="color:red" /*aria-hidden="true"*/></i>';
                        }
                    }
                }
            },
            {
                targets: 6,
                title: 'Attachment 6',
                data: 'attach6',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result6 == 'OK') {
                            return data + '&nbsp;<i class="fa fa-check" style="color:green" /*aria-hidden="true"*/></i>';
                        } else {
                            return data + '&nbsp;<i class="fa fa-times" style="color:red" /*aria-hidden="true"*/></i>';
                        }
                    }
                }
            },
            {
                targets: 7,
                title: 'Attachment 7',
                data: 'attach7',
                className: 'text-nowrap align-middle me-5',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        if (full.result7 == 'OK') {
                            return data + '&nbsp;<i class="fa fa-check" style="color:green" /*aria-hidden="true"*/></i>';
                        } else {
                            return data + '&nbsp;<i class="fa fa-times" style="color:red" /*aria-hidden="true"*/></i>';
                        }
                    }
                }
            },
            {
                targets: 8,
                title: 'Result',
                data: 'resultProgress',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data == 'success') {
                        return data + '&nbsp;<i class="fa fa-check" style="color:green" /*aria-hidden="true"*/></i>';
                    } else {
                        return data + '&nbsp;<i class="fa fa-times" style="color:red" /*aria-hidden="true"*/></i>';
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
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

$('#btn_download').on('click', function() {
   let url = "/tools/upload-attachment-promo-admin/download-template";

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_upload').on('click', function() {
    let e = document.querySelector("#btn_upload");
    let url = '/tools/upload-attachment-promo-admin/importData';
    $.ajax({
        url: url,
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
                    title: 'Upload Result',
                    html: "<div style='font-size:18pt;'>Success  : <b>" + result.resSuccesSum + "</b><br>Failed  : <b>" + result.resFailedSum + "</b></div><br><p>Short by column result to see failed upload</p><p>Check filename if there are failed result and refer to Special Character Rules</p>",
                    icon: "info",
                    confirmButtonText: "Ok",
                    allowOutsideClick: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(async function () {
                    $("#detail_result").removeClass("d-none");
                    dt_attachment_promo.clear().draw();
                    dt_attachment_promo.rows.add(result.data).draw();
                    Dropzone.forElement("#dropzone_promo_admin").removeAllFiles(true);
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
});

$('#btn_upload_send').on('click', function() {
    let e = document.querySelector("#btn_upload_send");
    let url = '/tools/upload-attachment-promo-admin/import';
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0
    blockUI.block();

    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        dataType: 'JSON',
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {
        },
        success: function (result, status, xhr, $form) {
            if (!result.error) {
                let data = result.data;
                if(data.length > 0) {
                    Swal.fire({
                        title: swalTitle,
                        text: `${data.length} promo will be process. Confirm?`,
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
                            readExcel(e);
                        } else {
                            blockUI.release();
                        }
                    })
                }
            } else {
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "error",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
                blockUI.release();
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
});

const readExcel = (e) => {
    $.ajax({
        url: "/tools/upload-attachment-promo-admin/read-excel",
        type: "GET",
        dataType: "JSON",
        success: async function (result) {
            if (!result.error) {
                let data = result.data;
                let elDisplayProgress = $('#d_progress');
                elDisplayProgress.removeClass('d-none');

                let perc = 0;
                for (let i=1; i <= data.length; i++) {
                    let dataRow = data[i-1];
                    await process(dataRow['promoNumber'], dataRow['attachment1'], dataRow['attachment2'], dataRow['attachment3'], dataRow['attachment4'], dataRow['attachment5']
                        , dataRow['attachment6'], dataRow['attachment7']);
                    perc = ((i / data.length) * 100).toFixed(0);
                    $('#text_progress').text(perc.toString() + '%');
                    let progress_import = $('#progress_bar');
                    progress_import.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());
                    if (i === data.length) {
                        progress_import.css('width', '0%').attr('aria-valuenow', '0');
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                        elDisplayProgress.addClass('d-none');
                        Swal.fire({
                            title: swalTitle,
                            text: 'Process Complete',
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                        mydropzone.removeAllFiles();
                        blockUI.release();
                    }
                }
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

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

const process = async (p_promo_number, p_attachment1, p_attachment2, p_attachment3, p_attachment4, p_attachment5, p_attachment6, p_attachment7 ) => {
    return new Promise(function (resolve, reject) {
        let formData = new FormData();
        formData.append('promoNumber', p_promo_number);
        formData.append('attachment1', p_attachment1);
        formData.append('attachment2', p_attachment2);
        formData.append('attachment3', p_attachment3);
        formData.append('attachment4', p_attachment4);
        formData.append('attachment5', p_attachment5);
        formData.append('attachment6', p_attachment6);
        formData.append('attachment7', p_attachment7);
        $.ajax({
            type        : 'POST',
            url         : "/tools/upload-attachment-promo-admin/process",
            data        : formData,
            dataType    : "JSON",
            async       : true,
            cache       : false,
            contentType : false,
            processData : false,
            success: function (result) {
                return resolve(result);
            },
            complete: function() {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
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
