#!/usr/bin/env bash

Trunk.App dimensions measure lines-of-code .
Trunk.App dimensions measure frequency-of-changes ./revisions.log

Trunk.App analyze hot-spots ./lines-of-code.csv ./frequencies.csv

Trunk.App visualize d3-hotspots hotspots.csv
