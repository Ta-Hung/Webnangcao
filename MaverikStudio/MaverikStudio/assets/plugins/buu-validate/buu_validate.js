function Validator(formSelector) {
    var formRules = {};
    var validatorRules = {
        required: function (value) {
            return value ? undefined : 'Không được để trống';
        },
        email: function (value) {
            var regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            return regex.test(value) ? undefined : 'Vui lòng nhập đúng định dạng email';
        },
        min: function (min) {
            return function (value) {
                return value.length >= min ? undefined : `Vui lòng nhập ít nhất ${min} ký tự`;
            }
        },
        max: function (max) {
            return function (value) {
                return value.length <= max ? undefined : `Vui lòng nhập ít hơn ${max} ký tự`;
            }
        }
    };

    var formElement = document.querySelector(formSelector);

    if (formElement) {
        var inputs = formElement.querySelectorAll('[name][rules]');

        for (var input of inputs) {
            var rules = input.getAttribute('rules').split('|');

            for (var rule of rules) {
                var ruleInfo;
                var isRuleHasValue = rule.includes(':');

                if (isRuleHasValue) {
                    ruleInfo = rule.split(':');
                    rule = ruleInfo[0];
                }

                var ruleFunc = validatorRules[rule];

                if (isRuleHasValue) {
                    ruleFunc = ruleFunc(ruleInfo[1]);
                }

                if (Array.isArray(formRules[input.name])) {
                    formRules[input.name].push(ruleFunc);
                } else {
                    formRules[input.name] = [ruleFunc];
                }
            }

            //Lắng nghe sự kiện để validate
            input.onblur = handleValidate;
            input.oninput = handleClear;
        }

        function handleValidate(event) {
            var rules = formRules[event.target.name];
            var errorMessage;
            for (var rule of rules) {
                errorMessage = rule(event.target.value);
                if (errorMessage) {
                    break;
                }
            }

            if (errorMessage) {
                var formGroup = event.target.closest('.form-group');
                var inputError = formGroup.querySelector('i');
                if (formGroup && !inputError.classList.contains('input-error')) {
                    inputError.classList.add('input-error');

                    var errMsg = document.createElement("span");
                    errMsg.className = 'error-message';
                    errMsg.innerText = errorMessage;

                    var iconInputError = document.createElement("div");
                    iconInputError.className = 'icon-input-error';
                    iconInputError.innerText = '!';
                    
                    formGroup.querySelector('.inputBox').appendChild(iconInputError);
                    formGroup.appendChild(errMsg);
                }
            }

            return !errorMessage;
        }

        function handleClear(event) {
            var formGroup = event.target.closest('.form-group');
            if (formGroup) {
                var inputError = formGroup.querySelector('i');
                if (inputError.classList.contains('input-error')) {
                    inputError.classList.remove('input-error');
                }
                
                var iconInputError = formGroup.querySelector('.inputBox .icon-input-error');
                if(iconInputError) {
                    iconInputError.remove();
                }

                var errMsg = formGroup.querySelector('.error-message');
                if(errMsg) {
                    errMsg.remove();
                }
            }
        }
    }

    //Xử lý submit form
    formElement.querySelector('button[type="submit"]').onclick = function(e) {
        e.preventDefault();

        var inputs = formElement.querySelectorAll('[name][rules]');
        var isValid = true;

        for (var input of inputs) {
            if (!handleValidate({ target: input })) {
                isValid = false
            }
        }

        //Khi không có lỗi thì submit form
        if (isValid) {
            formElement.submit();
        }
    }
}
