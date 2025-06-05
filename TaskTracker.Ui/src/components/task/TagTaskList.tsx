import React from "react";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskTag from "../tag/TaskTag";
import TaskList from "./TaskList";
import NoTasks from "./NoTasks";

const TagTaskList: React.FC = observer(() => {
    const { taskStore, tagStore } = useStore();

    const params = useParams();
    const tagId = Number(params.id);

    const tag = tagStore.tags.find(f => f.id === tagId);

    if (!tag) {
        return <NoTasks />;
    }

    const incompletedTasks = taskStore.getTagIncompletedTasks(tagId);
    const completedTasks = taskStore.getTagCompletedTasks(tagId);

    return (
        <>
            <div style={{ paddingLeft: 10, paddingTop: 10 }}>
                <TaskTag tag={tag} />
            </div>
            <Divider />
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
                shouldShowFolder={true}
            />
        </>
    );
});

export default TagTaskList;
