var quantityImage = $(".upload-image__destroy").siblings(".upload-image__item").length;

(function ($) {
    $.fn.filemanager = function (type, options) {

        this.on('click', function (e) {
            
            var target_input = $(this).children();
            var target_preview = $(this);

            const ckfinder = new CKFinder();
            ckfinder.selectActionFunction = function (url) {
                // set the value of the desired input to image url
                target_input.val('').val(url).trigger('change');

                target_preview.css("background-image", `url('${url}')`);

                // trigger change event
                target_preview.trigger('change');
            }

            ckfinder.popup();
        });
    }

    $.fn.filemanagerAdd = function (type, infinite, quantityAllow, customFlexBox, options) {

        this.on('click', function (e) {
            var boxInsert = $(this);

            const ckfinder = new CKFinder();
            ckfinder.selectActionFunction = function (url) {
                var newImage = boxInsert.parent().before(`<div class="upload-image__parent ${customFlexBox} pl-1 pr-1 mb-2">
                    <div class="upload-image__item-wrapper">
                    <div class="upload-image__item" style="background-image: url('${url}');">
                        <input class="form-control upload-image__holder" type="text" name="filepath[]" value="${url}" style="display: none;">
                    </div>
                    <div class="upload-image__destroy"></div>
                    </div>
                </div>`).prev();

                newImage.find(".upload-image__item").filemanager('image');
                newImage.find(".upload-image__destroy").click(function () {
                    $(this).parents('.upload-image__parent').remove();
                    quantityImage -= 1;

                    if ($(".upload-image__create").length == 0 && quantityImage < quantityAllow) {
                        $('.upload-image__wrapper').children().append(`<div class="upload-image__parent ${customFlexBox} pl-1 pr-1 mb-2">
                            <div class="upload-image__item-wrapper">
                                <div class="upload-image__item"></div>
                                <div class="upload-image__create"><i class="fa-thin fa-cloud-arrow-up"></i></div>
                            </div>
                        </div>`);

                        $(".upload-image__create").parent().filemanagerAdd('image', infinite, quantityAllow, customFlexBox);
                    }
                });

                quantityImage += 1;

                if (!infinite && quantityImage >= quantityAllow) {
                    $('.upload-image__create').parents('.upload-image__parent').remove();
                }
            }

            ckfinder.popup();
        });
    }
})(jQuery);

var buuUploadImage = function (infinite = true, quantityAllow = 1, customFlexBox = 'col-12') {
    $('.upload-image__destroy').click(function () {
        $(this).parents('.upload-image__parent').remove();
        quantityImage -= 1;

        if ($(".upload-image__create").length == 0 && quantityImage < quantityAllow) {
            $('.upload-image__wrapper').children().append(`<div class="upload-image__parent ${customFlexBox} pl-1 pr-1 mb-2">
                <div class="upload-image__item-wrapper">
                    <div class="upload-image__item"></div>
                    <div class="upload-image__create"><i class="fa-thin fa-cloud-arrow-up"></i></div>
                </div>
            </div>`);

            $(".upload-image__create").parent().filemanagerAdd('image', infinite, quantityAllow, customFlexBox);
        }
    });

    $(".upload-image__destroy").siblings(".upload-image__item").filemanager('image');

    if (!infinite && quantityImage >= quantityAllow) {
        $('.upload-image__create').parents('.upload-image__parent').remove();
    } else {
        $(".upload-image__create").parent().filemanagerAdd('image', infinite, quantityAllow, customFlexBox);
    }
}