.PHONY: purge clean test
.DEFAULT_GOAL := default

MACOSSLN	:= Barista.MacOS.sln
CORE		:= src/Barista.Core
MACOS		:= src/Barista.MacOS
BUILD		:= .build
TEST_CORE	:= test/Barista.Core.Tests

purge: clean
	rm -rf .vs
	rm -rf $(BUILD)

clean:
	rm -rf $(CORE)/obj
	rm -rf $(MACOS)/obj
	rm -rf $(TEST_CORE)/obj
	msbuild $(MACOSSLN) /t:Clean

build:
	msbuild $(MACOSSLN) /restore:True

default:
	msbuild $(MACOSSLN) /restore:True /p:Configuration=Release

test:
	dotnet test $(TEST_CORE)

install:
	open ./.build/bin/Barista.MacOS/Release/Barista-1.0.0.pkg