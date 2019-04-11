#!/bin/bash
rm -fr build && find . -iname bin -o -iname obj | xargs rm -rf
