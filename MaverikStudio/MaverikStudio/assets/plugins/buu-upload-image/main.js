var quantityImage = $(".upload-image__destroy").siblings(".upload-image__item").length;

(function( $ ){
    $.fn.filemanager = function(type, options) {
        type = type || 'file';

        this.on('click', function(e) {
        var route_prefix = (options && options.prefix) ? options.prefix : '/filemanager';
        var target_input = $(this).children();
        var target_preview = $(this);
        window.open(route_prefix + '?type=' + type, 'FileManager', 'width=900,height=600');
        window.SetUrl = function (items) {
            var file_path = items.map(function (item) {
                return item.url;
            }).join(',');

            // set the value of the desired input to image url
            target_input.val('').val(file_path).trigger('change');

            target_preview.css("background-image", `url('${file_path}')`);

            // trigger change event
            target_preview.trigger('change');
        };
        return false;
        });
    }

    $.fn.filemanagerAdd = function(type, infinite, quantityAllow, customFlexBox, options) {
        type = type || 'file';
    
        this.on('click', function(e) {
            var route_prefix = (options && options.prefix) ? options.prefix : '/filemanager';
            
            var boxInsert = $(this);
            window.open(route_prefix + '?type=' + type, 'FileManager', 'width=900,height=600');
            window.SetUrl = function (items) {
                var file_path = items.map(function (item) {
                    return item.url;
                }).join(',');
                
                var newImage =  boxInsert.parent().before(`<div class="upload-image__parent ${customFlexBox} pl-1 pr-1 mb-2">
                    <div class="upload-image__item-wrapper">
                    <div class="upload-image__item" style="background-image: url('${file_path}');">
                        <input class="form-control upload-image__holder" type="text" name="filepath[]" value="${file_path}" style="display: none;">
                    </div>
                    <div class="upload-image__destroy"></div>
                    </div>
                </div>`).prev();

                newImage.find(".upload-image__item").filemanager('image');
                newImage.find(".upload-image__destroy").click(function () {
                    $(this).parents('.upload-image__parent').remove();
                    quantityImage -= 1;
                    
                    if($(".upload-image__create").length == 0 && quantityImage < quantityAllow) {
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

                console.log(quantityImage, quantityAllow);
                if(!infinite && quantityImage >= quantityAllow) {
                    $('.upload-image__create').parents('.upload-image__parent').remove();
                }
            };
            return false;
        });
    }
})(jQuery);

var buuUploadImage = function (infinite = true, quantityAllow = 1, customFlexBox = 'col-12') {
    $('.upload-image__destroy').click(function () {
        $(this).parents('.upload-image__parent').remove();
        quantityImage -= 1;
        
        if($(".upload-image__create").length == 0 && quantityImage < quantityAllow) {
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

    if(!infinite && quantityImage >= quantityAllow) {
        $('.upload-image__create').parents('.upload-image__parent').remove();
    } else {
        $(".upload-image__create").parent().filemanagerAdd('image', infinite, quantityAllow, customFlexBox);
    }
}