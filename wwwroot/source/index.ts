export default function initialize(helper: any) {
    window.addEventListener('resize', event => {
        helper.invokeMethodAsync('OnMessage')
    })
}

declare global {
    interface Window {
        initialize: typeof initialize
    }
}

window.initialize = initialize