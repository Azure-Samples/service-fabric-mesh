#!/bin/sh

../scripts/linux/build_and_publish.sh helloworld.imageinfo.txt "$@" || {
    echo Failed to process image for helloworld
    exit 1
}

../scripts/linux/build_and_publish.sh helloworldsidecar.imageinfo.txt "$@" || {
    echo Failed to process image for helloworldsidecar
    exit 1
}

echo Successfully build helloworld images.
exit 0
