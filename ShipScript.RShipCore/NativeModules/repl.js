'use strict';
// ReSharper disable UndeclaredGlobalVariableUsing
const core = require('core');
const console = require('console');
const explore = require('explore');

function evaluate(command) {
    if (typeof command !== 'string') {
        return;
    }

    EngineInternal.evalCode = command;
    const success = core.eval();
    if (success) {
        explore(EngineInternal.evalResult);
    } else {
        console.err(EngineInternal.evalError);
    }
}

function hook(stream) {
    return stream.pipe(evaluate);
}

exports.hook = hook;
