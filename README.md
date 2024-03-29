# Elephants trunk

This project is a kind of a pet project used to reach a better understanding of concepts introduced by Adam Tornhill in his book "Your code as a Crime science".

I also wanted to have a tool which could help me perform a quick analysis of new projects. 

# How it works?

The analysis process consists of multiple steps.

1) Measure

Measures interesting dimensions like lines of code which correspond to complexity and frequency of change in a particular file

2) Analyze

Analyzes gathered in a previous step dimension to calculate a metric, like hot spots.

3) Visualize

Visualizes the output of the analysis. At the moment, the d3 visualization tool is used.

# How to run the analysis?

The preliminary step is to gather git log in a particular format. To do that, use this command:

```sh
git log --pretty=format:'[%h] %aN %ad %s' --date=short --numstat --no-merges --after=2022-01-01 -- '*.cs' > revisions.log
```

Note that the sample command filters commits by date and file extension. **Important** to use the script described in the next paragraph, the name of the output file must match.

The project is a command line tool. You can run steps independently or use a prepared script e.g. `hotspots.sh`.

```sh
#!/usr/bin/env bash
git log --pretty=format:'[%h] %aN %ad %s' --date=short --numstat --no-merges --after=2020-01-01 -- '*.cs' > revisions.log

Trunk.App dimensions measure lines-of-code .
Trunk.App dimensions measure frequency-of-changes revisions.log
Trunk.App analyze hot-spots ./lines-of-code.csv frequencies.csv
Trunk.App visualize d3 hotspots.csv
```

To visualize the result copy the `hotspot_proto.json` file to visualization folder. To run http server perform the command below in the visualization command:

```sh
python3 -m http.server 8888
```

# Sample visualisatoins

In the visualisation folder you may find useful visualisation pages

## Hot spots
Represents places in the source code that was under heavy development. It may indicate potential problematic areas in the code.

The size of the circle corresponds to the size of the file in the code base. The more red the circle is the more frequently the code has been changed. Big red circles represent hotspots.

![hot_spots](./img/hot_spots.png)

## Knowledge map
Represents the code ownership, which means that it represents the author who added the most lines to a particular module. The module granularity is a single folder.

Each circle represents a folder. The color of the last depth folder represents the author of most lines added to the folder.

![knowledge_map](./img/knowledge_map.png)

# Requirements

- .Net Sdk 6.0 (project build)
- python3 (for visualisation purposes)

# Contribution

As I mentioned, the project was created as a pet project. I followed the Pareto principle. If you are interested in participating in this project, feel free to make a PR. However, there are dozens of projects which make the job better than this one. 