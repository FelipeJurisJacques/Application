Object.prototype.callGetterAttribute = function (key) {
    return this[key]
}

Object.prototype.callSetterAttribute = function (key, value) {
    this[key] = value
}

Object.prototype.callSetterFunctionOnAttribute = function (dotNetObject, key, eventName) {
    this[key] = event => {
        dotNetObject.invokeMethodAsync(eventName, event)
    }
}

Object.prototype.callSetterEventListenerOnAttribute = function (dotNetObject, key, eventName) {
    window.addEventListener(key, event => {
        dotNetObject.invokeMethodAsync(eventName, event)
    })
}