#!/bin/bash

image_info_file=$1
if [ ! -e $image_info_file ]
then
    echo cannot find file "$image_info_file"
    exit 1
fi

registry_path=$2

found_entry=0
while IFS=$' ',$'\n'$'\r' read os image_name tag_value registry_tag_value build_root docker_file
do 
    if [ "$os" != "linux" ]
    then
        continue;
    fi
    found_entry=1

    echo image_name = $image_name
    echo tag_value = $tag_value
    echo registry_tag_value = $registry_tag_value
    echo registry_path = $registry_path
    echo build_root = $build_root
    echo docker_file = $docker_file
    echo 

    echo Building Dev Image
    docker_command="docker build $build_root -f $docker_file -t $image_name:dev-$tag_value"
    echo $docker_command
    eval $docker_command || { echo Failed to build image $image_name:dev-$tag_value; exit 1; }
    echo Successfully built image $image_name:dev-$tag_value
    echo

    if [ "$registry_path" = "" ]
    then
        continue
    fi

    echo Tagging image
    docker_command="docker tag $image_name:dev-$tag_value $registry_path/$image_name:$registry_tag_value"
    echo $docker_command
    eval $docker_command || { echo Failed to tag image $registry_path/$image_name:$registry_tag_value; exit 1; }
    echo Successfully tagged image $registry_path/$image_name:$registry_tag_value
    echo

    echo Publishing image
    docker_command="docker push $registry_path/$image_name:$registry_tag_value"
    echo $docker_command
    eval $docker_command || { echo Failed to publish image to $registry_path/$image_name:$registry_tag_value; exit 1; }

    echo Successfully published image to $registry_path/$image_name:$registry_tag_value
    echo
done < $image_info_file

if [ $found_entry -ne 1 ]
then
    echo Could not find information for "linux" in $image_info_file
    exit 1
fi
