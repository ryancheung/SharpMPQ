# MIT License - Copyright (C) ryancheung
# This file is subject to the terms and conditions defined in
# file 'LICENSE', which is part of this source code package.

cmake_minimum_required(VERSION 3.11)

project(libmpq)

# build in Release-mode by default if not explicitly set
if(CMAKE_GENERATOR STREQUAL "Ninja Multi-Config")
  set(CMAKE_DEFAULT_BUILD_TYPE "RelWithDebInfo" CACHE INTERNAL "")
endif()
if(NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE "RelWithDebInfo")
endif()

add_library(compile-option-interface INTERFACE)

if(UNIX)
  target_compile_options(compile-option-interface
    INTERFACE
      -fPIC
      -Wno-attributes)
endif()

# An interface library to make the target features available to other targets
add_library(feature-interface INTERFACE)

target_compile_features(feature-interface
  INTERFACE
    cxx_std_17
)

add_library(no-warning-interface INTERFACE)

if(CMAKE_CXX_COMPILER_ID STREQUAL "MSVC")
  target_compile_options(no-warning-interface
    INTERFACE
      /W0)
else()
  target_compile_options(no-warning-interface
    INTERFACE
      -w)
endif()

add_library(dependency-interface INTERFACE)

target_link_libraries(dependency-interface
  INTERFACE
    compile-option-interface
    feature-interface
    no-warning-interface)

add_subdirectory(bzip2)
add_subdirectory(zlib)
add_subdirectory(libmpq)
