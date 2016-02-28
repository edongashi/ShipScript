'use strict';
// ReSharper disable UndeclaredGlobalVariableUsing
const nativeProp = '{c2cf47d3-916b-4a3f-be2a-6ff567425808}';
const evalNative = true;

function createExplore(output, color) {
    function printString(str, longString) {
        if (!longString && str.length > 150) {
            const substr = str.substring(0, 150);
            output.write(`'${substr}`, color.dgreen);
            output.write('...', color.dgray);
            output.write('\'', color.dgreen);
        } else {
            output.write(`'${str}'`, color.dgreen);
        }
    }

    function printSimple(obj, longString) {
        if (obj === undefined) {
            output.write('undefined', color.dgray);
            return true;
        }

        if (obj === null) {
            output.write('null', color.white);
            return true;
        }

        if (EngineInternal.isVoid(obj)) {
            output.write('void', color.dmagenta);
            return true;
        }

        const type = typeof obj;
        if (type === 'string') {
            printString(obj, longString);
            return true;
        }

        if (type === 'number' || type === 'boolean' || obj instanceof RegExp) {
            output.write(obj.toString(), color.dyellow);
            return true;
        }

        return false;
    }

    function printObject(obj, parent) {
        if (obj === parent) {
            output.write('[Circular]', color.dcyan);
            return true;
        }

        if (obj === global) {
            output.write('[Global]', color.dcyan);
            return true;
        }

        if (!obj.constructor || !obj.constructor.name) {
            output.write('[Unknown]', color.dred);
            return true;
        }

        const name = obj.constructor.name;
        if (obj.hasOwnProperty(nativeProp)) {
            if (name === "HostDelegate") {
                output.write('[Method]', color.dmagenta);
                return true;
            }

            output.write('[' + name, color.dmagenta);
            try {
                const typeName = obj.GetType().Name;
                if (typeName) {
                    output.write(':' + typeName, color.dmagenta);
                }
            } catch (err) {
                // ignored
            }

            output.write(']', color.dmagenta);
            return true;
        }

        output.write(`[${name}]`, color.dcyan);
        return true;
    }

    function explore(obj, evalGetters) {
        const printed = printSimple(obj, true);
        if (printed) {
            output.writeln();
            return;
        }

        if (obj instanceof Array) {
            let cut = false;
            let len = obj.length - 1;
            if (len === -1) {
                output.writeln('[]');
                return;
            }

            if (len > 1000) {
                len = 1000;
                cut = true;
            }

            output.write('[ ');
            let arrElement;
            for (let i = 0; i < len; i++) {
                arrElement = obj[i];
                printSimple(arrElement) || printObject(arrElement, obj);
                output.write(', ');
            }

            if (cut) {
                output.write('... ');
            }

            arrElement = obj[obj.length - 1];
            printSimple(arrElement) || printObject(arrElement, obj);
            output.writeln(' ]');
            return;
        }

        printObject(obj);
        var properties;
        const isNative = obj.hasOwnProperty(nativeProp);
        if (isNative) {
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
            output.writeln();
            return;
        }

        output.write(' {');
        output.writeln();
        for (let i = 0; i < propertiesLength; i++) {
            let key = properties[i];
            output.write(`  ${key}: `);
            if (isNative && !evalNative) {
                output.write('[Native]', color.dmagenta);
            } else {
                try {
                    const descriptor = Object.getOwnPropertyDescriptor(obj, key);
                    if (descriptor.get) {
                        if (evalGetters) {
                            const val = descriptor.get();
                            printSimple(val) || printObject(val, obj);
                        } else {
                            output.write('[Getter]', color.dcyan);
                        }
                    } else {
                        const val = descriptor.value;
                        printSimple(val) || printObject(val, obj);
                    }
                } catch (err) {
                    output.write('[Error]', color.dred);
                }
            }

            if (i < commas) {
                output.writeln(',');
            } else {
                output.writeln();
            }
        }

        output.writeln('}');
    }

    return explore;
}

exports.create = createExplore;
