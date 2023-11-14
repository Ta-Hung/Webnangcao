//Handle slider
const HandleSlider = (idSlider , interval = 0, leftOrRight = true, controlLeftOrg = '.slider-control-left', controlRightOrg = '.slider-control-right', sliderListItemOrg = '.slider-list-item', sliderItemsOrg = '.slider-item') => {
    let controlLeft = document.querySelector(idSlider + '>' + controlLeftOrg);
    let controlRight = document.querySelector(idSlider + '>' + controlRightOrg);
    let sliderListItem = document.querySelector(idSlider + '>' + sliderListItemOrg);
    let sliderItems = sliderListItem.querySelectorAll(sliderItemsOrg);
    let enableClickLeft = true;
    let enableClickRight = true;

    let firstSliderItem = sliderListItem.querySelector(sliderItemsOrg + ':first-child');

    sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0.25s ease 0s;`;

    window.addEventListener('resize', () => {
        firstSliderItem = sliderListItem.querySelector(sliderItemsOrg + ':first-child');
        sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0.25s ease 0s;`;
    })

    sliderListItem.addEventListener("transitionend", function(event) {
        if (event.propertyName === "transform") {
            let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

            let translateXValue = match ? parseFloat(match[1]) : 0;

            if(translateXValue <= -(firstSliderItem.offsetWidth*(sliderItems.length/3*2))) {
                sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0s ease 0s;`;
            }

            if(translateXValue >= 0) {
                sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0s ease 0s;`;
            }

            enableClickRight = true;
            enableClickLeft = true;
        }
    });

    //Sang trái dương
    controlLeft.addEventListener('click', (e) => {
        e.stopPropagation();
        if(enableClickLeft) {
            let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

            let translateXValue = match ? parseFloat(match[1]) : 0;

            sliderListItem.style = `transform: translateX(${translateXValue + firstSliderItem.offsetWidth}px); transition: all 0.25s ease 0s;`;

            enableClickLeft = false;

            if(setTime) {
                clearInterval(setTime);

                if(interval > 0) {
                    setTime = setInterval(() => {
                        if(leftOrRight) {
                            controlLeft.click();
                        } else {
                            controlRight.click();
                        }
                    }, interval);
                }
            }
        }
    })

    controlLeft.addEventListener('mousedown', (e) => {
        e.stopPropagation();
    })

    //Sang phải âm
    controlRight.addEventListener('click', (e) => {
        e.stopPropagation();
        if(enableClickRight) {
            // Sử dụng regex để trích xuất giá trị translateX
            let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

            // Nếu có kết quả, lấy giá trị từ nhóm nằm trong dấu ngoặc
            let translateXValue = match ? parseFloat(match[1]) : 0;

            sliderListItem.style = `transform: translateX(${translateXValue - firstSliderItem.offsetWidth}px); transition: all 0.25s ease 0s;`;

            enableClickRight = false;

            if(setTime) {
                clearInterval(setTime);

                if(interval > 0) {
                    setTime = setInterval(() => {
                        if(leftOrRight) {
                            controlLeft.click();
                        } else {
                            controlRight.click();
                        }
                    }, interval);
                }
            }
        }
    })

    let setTime;
    if(interval > 0) {
        setTime = setInterval(() => {
            if(leftOrRight) {
                controlLeft.click();
            } else {
                controlRight.click();
            }
        }, interval);
    }

    controlRight.addEventListener('mousedown', (e) => {
        e.stopPropagation();
    })

    //Handle grabbing pull slider
    let isDragging;
    let offsetX;

    sliderListItem.addEventListener("mousedown", function(e) {
        console.log(e.clientX);
        console.log(sliderListItem.getBoundingClientRect().left);
        isDragging = true;
        offsetX = e.clientX - (sliderListItem.getBoundingClientRect().left - (document.documentElement.clientWidth - sliderListItem.offsetWidth)/2);
        sliderListItem.classList.add('pull');
    });

    document.addEventListener("mousemove", function(e) {
        if (isDragging) {
            let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

            let translateXValue = match ? parseFloat(match[1]) : 0;

            if(translateXValue <= -(firstSliderItem.offsetWidth*(sliderItems.length/3*2))) {
                sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0s ease 0s;`;
                offsetX = e.clientX - sliderListItem.getBoundingClientRect().left;
                return;
            } else if(translateXValue >= 0) {
                sliderListItem.style = `transform: translateX(-${firstSliderItem.offsetWidth * (sliderItems.length/3)}px); transition: all 0s ease 0s;`;
                offsetX = e.clientX - sliderListItem.getBoundingClientRect().left;
                return;
            }

            let x = e.clientX - offsetX;
            sliderListItem.style.transform = "translateX(" + x + "px)";
        }
    });

    document.addEventListener("mouseup", function() {
        isDragging = false;
        sliderListItem.classList.remove('pull');

        let match = (sliderListItem.style.transform).match(/translateX\(([^)]+)\)/);

        let translateXValue = match ? parseFloat(match[1]) : 0;
        
        let result = translateXValue/firstSliderItem.offsetWidth;

        let currentPosition = 0;
        if((result%1) >= 0) {
            currentPosition = (result%1) >= 0.5 ? Math.ceil(result) : Math.floor(result);
        } else {
            currentPosition = (result%1) >= -0.5 ? Math.ceil(result) : Math.floor(result);
        }

        let x = currentPosition * firstSliderItem.offsetWidth;

        sliderListItem.style = `transform: translateX(${x}px); transition: all 0.25s ease 0s;`;
    });
}