// Hàm khởi tạo mặc định cho datepicker, select2, popup,...
$(document).ready(function () {
    defaultConfig(this);
});

// Khởi tạo các thành phần UI như datepicker, select2, popup,...
function defaultConfig(parent) {
    parent?.querySelectorAll('.datepicker').forEach(function (e) {
        $(e).datepicker({
            dateFormat: 'dd/mm/yy',
            autoclose: true
        });
    });

    parent?.querySelectorAll('.select2').forEach(function (e) {
        $(e).select2({
            width: "100%",
            theme: 'bootstrap-5'
        });
    });

    parent?.querySelectorAll('.popup').forEach(function (e) {
        $(e).on("click", function (event) {
            event.preventDefault();
            modelPopup(this);
        });
    });

    parent?.querySelectorAll('.select-value').forEach(function (e) {
        $(e).on("change", function () {
            this.form.submit();
        });
    });
}

// Disable/Enable nút trong form
function ControlDisabled(formId) {
    $('#' + formId).find(':button').prop('disabled', true);
}
function ControlEnabled(formId) {
    $('#' + formId).find(':button').prop('disabled', false);
}

// Gửi form dạng POST bằng FormData
function postData(formId, postUrl, callBack) {
    $('#' + formId).off('submit').on('submit', function (e) {
        ControlDisabled(formId);
        if ($("#" + formId).valid()) {
            e.preventDefault();
            let formData = new FormData(this);
            showLoading();
            $.ajax({
                url: postUrl,
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    hideLoading();
                    callBack(data);
                    ShowMessageData(data);
                    if (data.isSuccessed === true) {
                        dropModal();
                    } else {
                        ControlEnabled(formId);
                    }
                },
                error: function () {
                    hideLoading();
                    ControlEnabled(formId);
                }
            });
        } else {
            ControlEnabled(formId);
        }
    });
}

// Gửi dữ liệu dạng object
function postValue(postUrl, dataInput, callBack) {
    showLoading();
    $.ajax({
        url: postUrl,
        type: "POST",
        data: dataInput,
        success: function (data) {
            hideLoading();
            callBack(data);
            if (data.isSuccessed === true) ShowMessageData(data);
        },
        error: function () {
            hideLoading();
        }
    });
}

// Load nội dung vào một phần tử
function loadContent(url, element, callback = null) {
    showLoading();
    $.ajax({
        url: decodeURIComponent(url),
        type: "GET",
        success: function (data) {
            hideLoading();
            if (data) {
                $(element).html(data);
                defaultConfig(element);
            }
            if (callback) callback();
        },
        error: function () {
            hideLoading();
            if (callback) callback();
        }
    });
}

// Modal xử lý popup
function modelPopup(triggerElement) {
    const url = $(triggerElement).data('url');
    const input = $(triggerElement).data('link');
    const type = $(triggerElement).data('type');
    const data = getUrlVars(decodeURIComponent(input));
    const modal = new bootstrap.Modal(document.getElementById("modal"));

    $('#modal .modal-dialog').removeClass('modal-sm modal-lg modal-xl').html('');

    $.ajax({
        url: url,
        type: "GET",
        data: data,
        success: function (data) {
            if (data?.isSuccessed === false) {
                dropModal();
                ShowMessageData(data);
            } else {
                $('#modal .modal-dialog').html(data);
                if (type) $('#modal .modal-dialog').addClass(type);
                modal.show();
            }
            hideLoading();
        },
        error: function () {
            $(".modal-backdrop").remove();
            modal.hide();
            dropModal();
            hideLoading();
        }
    });
}

// Đóng modal popup
function dropModal() {
    $('#modal').modal('hide');
    $('#modal .modal-dialog').removeClass('modal-sm modal-lg modal-xl').html('');
}

// Helper xử lý URL thành object
function getUrlVars(url) {
    const result = {};
    if (!url) return result;
    result.resultUrl = url;
    url.slice(url.indexOf('?') + 1).split('&').forEach(item => {
        const [key, val] = item.split('=');
        result[key] = val?.replace("+", " ") ?? "";
    });
    return result;
}

// Hiển thị thông báo mặc định
function ShowMessageData(data) {
    if (data?.isSuccessed === false) {
        Swal.fire({
            icon: "error",
            title: "Lỗi",
            text: data.message || "Cập nhật không thành công"
        });
    } else {
        Swal.fire({
            icon: "success",
            title: "Thành công",
            text: data.message || "Thực hiện thành công"
        });
    }
}


// Loading UI xử lý
function showLoading() {
    $('.wait').addClass("loading");
}
function hideLoading() {
    $('.wait').removeClass("loading");
}

// Kiểm tra chuỗi rỗng hoặc null
function isNullOrWhiteSpace(str) {
    return !str || typeof str !== 'string' || str.trim() === "";
}

// Chuyển dấu tiếng Việt sang không dấu
function convertViToEn(str) {
    return str.toLowerCase()
        .replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a")
        .replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e")
        .replace(/ì|í|ị|ỉ|ĩ/g, "i")
        .replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o")
        .replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u")
        .replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y")
        .replace(/đ/g, "d")
        .replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, "")
        .replace(/\u02C6|\u0306|\u031B/g, "");
}

// Tạo slug/route url từ tiếng Việt
function convertToReplaceRoute(str) {
    const words = str.split(' ');
    const cut = words.length > 40 ? words.slice(0, 40) : words;
    let route = convertViToEn(cut.join(' ').trim());
    return route.replace(/\W+/g, '-');
}

// Debounce cho các sự kiện cần delay
function debounce(fn, ms) {
    let timer;
    return function (...args) {
        clearTimeout(timer);
        timer = setTimeout(() => fn.apply(this, args), ms);
    };
}
// Hàm xóa dữ liệu
function DeleteData(url, id, callback) {
    const token = $('input[name="__RequestVerificationToken"]').val(); // Lấy token từ form hiện tại

    $.ajax({
        url: url,
        type: 'POST',
        data: {
            __RequestVerificationToken: token,
            id: id
        },
        success: function (res) {
            if (res.success) {
                Swal.fire('Đã xóa!', res.message, 'success');
                if (callback) callback();
            } else {
                Swal.fire('Lỗi', res.message || 'Không thể xóa.', 'error');
            }
        },
        error: function () {
            Swal.fire('Lỗi', 'Xóa thất bại do lỗi máy chủ.', 'error');
        }
    });
}

