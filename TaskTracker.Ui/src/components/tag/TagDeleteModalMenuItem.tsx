import React from "react";
import { observer } from "mobx-react-lite";
import { useTranslation } from "../../hooks/useTranslation";
import ITag from "../../interfaces/ITag";
import TagDeleteModal from "./TagDeleteModal";

interface ITagDeleteModalMenuItemProps {
    tag: ITag;
}

const TagDeleteModalMenuItem: React.FC<ITagDeleteModalMenuItemProps> = observer(
    props => {
        const t = useTranslation();

        const [isModalOpen, setIsModalOpen] = React.useState(false);

        const handleDeleteTagButtonClick = () => {
            setIsModalOpen(true);
        };

        const hideTagDeleteModal = () => {
            setIsModalOpen(false);
        };

        return (
            <>
                <div onClick={handleDeleteTagButtonClick}>{t("deleteTag")}</div>
                <TagDeleteModal
                    tag={props.tag}
                    isModalOpen={isModalOpen}
                    hideModal={hideTagDeleteModal}
                />
            </>
        );
    }
);

export default TagDeleteModalMenuItem;
