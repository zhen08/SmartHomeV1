#!/bin/bash
thrift -gen csharp smartvideo.thrift
rm ./Transport/*
cp ./gen-csharp/SmartVideo/Transport/* ./Transport/
rm -rf ./gen-csharp/
