'use strict';
// ReSharper disable UndeclaredGlobalVariableUsing
const stdout = require('stdout');
var color = stdout.color;
const nativeProp = '{c2cf47d3-916b-4a3f-be2a-6ff567425808}';

{
    let copy = {};
    Object.assign(copy, color);
    color = copy;
}

function printString(str, longString) {
    if (!longString && str.length > 150) {
        var substr = str.substring(0, 150);
        stdout.write(`'${substr}`, color.dgreen);
        stdout.write('...', color.dgray);
        stdout.write('\'', color.dgreen);
    } else {
        stdout.write(`'${str}'`, color.dgreen);
    }
}

function printSimple(obj, longString) {
    if (obj === undefined) {
        stdout.write('undefined', color.dgray);
        return true;
    }

    if (obj === null) {
        stdout.write('null', color.white);
        return true;
    }

    if (EngineInternal.isVoid(obj)) {
        stdout.write('void', color.dmagenta);
        return true;
    }

    const type = typeof obj;
    if (type === 'string') {
        printString(obj, longString);
        return true;
    }

    if (type === 'number' || type === 'boolean' || obj instanceof RegExp) {
        stdout.write(obj.toString(), color.dyellow);
        return true;
    }

    return false;
}

function printObject(obj, parent) {
    if (obj === parent) {
        stdout.write('[Circular]', color.dcyan);
        return true;
    }

    if (obj === global) {
        stdout.write('[Global]', color.dcyan);
        return true;
    }

    if (!obj.constructor || !obj.constructor.name) {
        stdout.write('[Unknown]', color.dred);
        return true;
    }

    const name = obj.constructor.name;
    if (obj.hasOwnProperty(nativeProp)) {
        if (name === "HostDelegate") {
            stdout.write('[Method]', color.dmagenta);
            return true;
        }

        stdout.write('[' + name, color.dmagenta);
        try {
            const typeName = obj.GetType().Name;
            if (typeName) {
                stdout.write(':' + typeName, color.dmagenta);
            }
        } catch (err) {
            // ignored
        }

        stdout.write(']', color.dmagenta);
        return true;
    }

    stdout.write(`[${name}]`, color.dcyan);
    return true;
}

function explore(obj, evalGetters) {
    const printed = printSimple(obj, true);
    if (printed) {
        stdout.writeln();
        return;
    }

    if (obj instanceof Array) {
        let cut = false;
        let len = obj.length - 1;
        if (len === -1) {
            stdout.writeln('[]');
            return;
        }

        if (len > 1000) {
            len = 1000;
            cut = true;
        }

        stdout.write('[ ');
        let arrElement;
        for (let i = 0; i < len; i++) {
            arrElement = obj[i];
            printSimple(arrElement) || printObject(arrElement, obj);
            stdout.write(', ');
        }

        if (cut) {
            stdout.write('... ');
        }

        arrElement = obj[obj.length - 1];
        printSimple(arrElement) || printObject(arrElement, obj);
        stdout.writeln(' ]');
        return;
    }

    printObject(obj);
    stdout.write(' {');
    var properties;
    if (obj.hasOwnProperty(nativeProp)) {
        properties = [];
        const keys = Object.keys(obj);
        const keysLength = keys.length;
        for (let i = 0; i < keysLength; i++) {
            let key = keys[i];
            if (key === 'ToString' || key === 'GetHashCode' || key === 'GetType' || key === 'Equals') {
                continue;
            }
            properties.push(key);
        }
    } else {
        properties = Object.keys(obj);
    }

    const propertiesLength = properties.length;
    const commas = propertiesLength - 1;
    if (propertiesLength === 0) {
        stdout.writeln('}');
        return;
    }

    stdout.writeln();
    for (let i = 0; i < propertiesLength; i++) {
        let key = properties[i];
        stdout.write(`  ${key}: `);
        try {
            const descriptor = Object.getOwnPropertyDescriptor(obj, key);
            if (descriptor.get) {
                if (evalGetters) {
                    const val = descriptor.get();
                    printSimple(val) || printObject(val, obj);
                } else {
                    stdout.write('[Getter]', color.dcyan);
                }
            } else {
                const val = descriptor.value;
                printSimple(val) || printObject(val, obj);
            }
        }
        catch (err) {
            stdout.write('[Error]', color.dred);
        }

        if (i < commas) {
            stdout.writeln(',');
        } else {
            stdout.writeln();
        }
    }

    stdout.writeln('}');
}

module.exports = explore;
