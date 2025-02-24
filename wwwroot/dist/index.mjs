function initialize(helper) {
    window.addEventListener('resize', event => {
        helper.invokeMethodAsync('OnMessage');
    });
}
window.initialize = initialize;

export { initialize as default };
