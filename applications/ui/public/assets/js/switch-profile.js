'use strict';
var swalTitleSwitch = "Switch Profile";

var targetSwitch = document.querySelector(".card_switch_profile");
var blockUISwitch = new KTBlockUI(targetSwitch, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('form').submit(false);
})(window, document, jQuery);

$('#btn_switch_profile').on('click', async function () {
    await getListProfile();
    $('#modal_popup_profile_change').modal('show');
});

$('#change_profile').on('change', function () {
    if ($(this).val() !== "") {
        var formData = new FormData();
        formData.append('profileId', $(this).val())
        let url = '/auth/profile/push-session';
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
                    blockUISwitch.block();
                },
                success: function (result, status, xhr, $form) {
                    if (!result.error) {
                        window.location.href = '/';
                    } else {
                        Swal.fire({
                            title: swalTitleSwitch,
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "OK",
                            customClass: { confirmButton: "btn btn-primary" }
                        });
                    }
                },
                complete: function () {

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.message);
                    Swal.fire({
                        title: swalTitleSwitch,
                        text: "Change profile failed",
                        icon: "error",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        confirmButtonText: "OK",
                        customClass: { confirmButton: "btn btn-primary" }
                    });
                }
            });
        });
    }
});

const getListProfile = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/auth/profile",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                let data = [];
                for (var j = 0, len = result.data.length; j < len; ++j) {
                    data.push({
                        id: result.data[j].profileid,
                        text: result.data[j].profileid
                    });
                }
                $('#change_profile').select2({
                    placeholder: "Select a profile",
                    width: '100%',
                    data: data
                });
                resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == 403) {
                    Swal.fire({
                        title: swalTitleSwitch,
                        text: jqXHR.responseJSON.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-primary" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                reject(jqXHR.responseText);
                console.log(jqXHR.responseText);
            }
        });
    });
}

