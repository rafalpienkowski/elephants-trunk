#!/usr/bin/env bash
git log --pretty=format:'[%h] %aN %ad %s' --date=short --numstat --no-merges --after=2020-01-01 -- '*.cs' > revisions.log

Trunk.App dimensions measure lines-of-code .
Trunk.App dimensions measure frequency-of-changes revisions.log

Trunk.App analyze hot-spots lines-of-code.csv frequencies.csv

Trunk.App visualize d3-hotspots hotspots.csv
 