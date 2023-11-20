/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.filebrowserBrowseUrl = '/assets/plugins/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl  = '/assets/plugins/ckfinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl  = '/assets/plugins/ckfinder/ckfinder.html?type=Flash';
    config.filebrowserUploadUrl  = '/assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl  = '/assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl  = '/assets/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    config.filebrowserWindowWidth  = '1000';
    config.filebrowserWindowHeight = '700';
};
