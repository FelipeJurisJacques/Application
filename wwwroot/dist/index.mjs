function listenResize(helper) {
    helper.invokeMethodAsync('OnResize', screen.width, screen.height);
    window.addEventListener('resize', () => {
        helper.invokeMethodAsync('OnResize', screen.width, screen.height);
    });
}
window.listenResize = listenResize;

export { listenResize as default };
